using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.Vehicle.StartVehicle;
internal class StartVehicleProcessManager : IStartVehicleProcessManager
{
    private readonly IVehicleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;
    private int _operatorId;
    private int _vehicleId;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public bool Running => !_trackerCollection.AllFinishedOrFailed;

    public bool FinishedSuccessful => !_trackerCollection.AllRequiredSucceded;

    public StartVehicleProcessManager(IVehicleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void PublishEventIfPossible()
    {
        if (_trackerCollection.AllFinishedOrFailed)
        {
            Result result = !_trackerCollection.Failed ? new SuccessResultNoData() : new InvalidResultNoData(_errors.ToArray());
            ProcesserFinished @event = new(result, ProcessManagerId);
            foreach (var handler in _handlers)
            {
                handler.Invoke(@event);
            }
        }
    }

    public void RegistrateHandler(Action<ProcesserFinished> handler)
    {
        _handlers.Add(handler);
    }

    public void SetUp(Guid correlationId)
    {
        if(CorrelationId == default)
        {
            CorrelationId = correlationId; //maybe have an event for the attempt start command instead of these four here
            //the event, if successed, will set the needed ids and these four here.
            _trackerCollection.AddEventTracker<AttemptToStartVehicleStarted>(true, DomainEventType.Succeeder);
        }
    }

    public void Handler(OperatorNotFound @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorNotFound>();
        _trackerCollection.FailEvent<OperatorWasFound>();
        //command to send depends on if, at this point, the other was found has failed, completed or awaiting.
        //if the VehicleWasFound is complete order vehicle to remove operator id
        //if neither was found nothing extra is needed to be done
        if (_trackerCollection.HasEventCompleted<VehicleWasFound>() is true)
        { //set event tracker for this
            _commandBus.Dispatch(new RemoveOperatorFromVehicle(_vehicleId, _operatorId, CorrelationId, @event.EventId));
        }

        _errors.AddRange(@event.Errors);

        PublishEventIfPossible();
    }

    public void Handler(VehicleNotFound @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<VehicleNotFound>();
        _trackerCollection.FailEvent<VehicleWasFound>();
        //command to send depends on if, at this point, the other was found has failed, completed or awaiting.
        //if the OperatorWasFound is complete order operator to remove vehicle id
        //if neither was found nothing extra is needed to be done
        if (_trackerCollection.HasEventCompleted<OperatorWasFound>() is true)
        { //set event tracker for this
            _commandBus.Dispatch(new RemoveVehicleFromOperator(_vehicleId, _operatorId, CorrelationId, @event.EventId));
        }

        _errors.AddRange(@event.Errors);

        PublishEventIfPossible();
    }

    public void Handler(OperatorWasFound @event)
    {
        if (CorrelationId != CorrelationId) { return; }
        //command to send depends on if, at this point, the other was found has failed, completed or awaiting.
        //if other not found, order this one to remove its vehicle id (to ensure it is not present)
        //if both were found order it to check permissions
        _trackerCollection.CompleteEvent<OperatorWasFound>();
        _trackerCollection.RemoveEvent<OperatorNotFound>();

        if (_trackerCollection.HasEventCompleted<VehicleWasFound>() is true)
        {
            //set the different event trackers needed
            _trackerCollection.AddEventTracker<PermittedToOperate>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<OperatorLackedNeededLicense>(false, DomainEventType.Failer);
            _trackerCollection.AddEventTracker<OperatorLicenseExpired>(false, DomainEventType.Failer);
            _trackerCollection.AddEventTracker<NotPermittedToOperate>(false, DomainEventType.Failer);
            _commandBus.Dispatch(new CheckPermissions(_operatorId, _vehicleId, CorrelationId, @event.EventId));
        }
        else if (_trackerCollection.HasEventFailed<VehicleWasFound>() is true)
        { //set event tracker
            _commandBus.Dispatch(new RemoveVehicleFromOperator(_vehicleId, _operatorId, CorrelationId, @event.EventId));
        }
        
        throw new NotImplementedException();
    }

    public void Handler(VehicleWasFound @event)
    {
        if (CorrelationId != CorrelationId) { return; }
        //command to send depends on if, at this point, the other was found has failed, completed or awaiting.
        //if other not found, order this one to remove its operator id (to ensure it is not present)
        //if both were found order it to check permissions
        _trackerCollection.CompleteEvent<VehicleWasFound>();


        if (_trackerCollection.HasEventCompleted<OperatorWasFound>() is true)
        {
            //set the different event trackers needed
            _trackerCollection.AddEventTracker<PermittedToOperate>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<OperatorLackedNeededLicense>(false, DomainEventType.Failer);
            _trackerCollection.AddEventTracker<OperatorLicenseExpired>(false, DomainEventType.Failer);
            _trackerCollection.AddEventTracker<NotPermittedToOperate>(false, DomainEventType.Failer);
            _commandBus.Dispatch(new CheckPermissions(_operatorId, _vehicleId, CorrelationId, @event.EventId));
        }
        else if (_trackerCollection.HasEventFailed<OperatorWasFound>() is true)
        {
            _trackerCollection.AddEventTracker<VehicleRemovedOperator>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<VehicleNotRequiredToRemoveOperator>(true, DomainEventType.Succeeder);
            _commandBus.Dispatch(new RemoveOperatorFromVehicle(_vehicleId, _operatorId, CorrelationId, @event.EventId));
        }


        throw new NotImplementedException();
    }

    public void Handler(VehicleStartedSucceeded @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }

    public void Handler(VehicleStartedFailed @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }

    public void Handler(NotPermittedToOperate @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<NotPermittedToOperate>();
        _trackerCollection.FailEvent<PermittedToOperate>();
        _trackerCollection.RemoveEvent<OperatorLackedNeededLicense>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();

        PublishEventIfPossible();
    }

    public void Handler(PermittedToOperate @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PermittedToOperate>();
        _trackerCollection.RemoveEvent<NotPermittedToOperate>();
        _trackerCollection.RemoveEvent<OperatorLackedNeededLicense>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();

        _trackerCollection.AddEventTracker<VehicleStartedSucceeded>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<VehicleStartedFailed>(false, DomainEventType.Failer);
        _commandBus.Dispatch(new DL.Models.Vehicles.CQRS.Commands.StartVehicle(_vehicleId, CorrelationId, @event.EventId));

        //no reason to check if finished in these methods that dispatch commands
    }

    public void Handler(OperatorLackedNeededLicense @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLackedNeededLicense>();
        _trackerCollection.RemoveEvent<NotPermittedToOperate>();
        _trackerCollection.FailEvent<PermittedToOperate>(); //consider setting this failed instead to ensure a InvalidResultNoData is transmitted back
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>(); //either that or have an success/error event type enum that is used in the event tracker collection, e.g. EventType.Succeder, EventType.Failer

        //remember events
        _trackerCollection.AddEventTracker<VehicleRemovedOperator>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<VehicleNotRequiredToRemoveOperator>(true, DomainEventType.Succeeder);
        _commandBus.Dispatch(new RemoveOperatorFromVehicle(_vehicleId, _operatorId, CorrelationId, @event.EventId));
        //remember events
        _trackerCollection.AddEventTracker<OperatorRemovedVehicle>(true, DomainEventType.Succeeder);
        _commandBus.Dispatch(new RemoveVehicleFromOperator(_vehicleId, _operatorId, CorrelationId, @event.EventId));

    }

    public void Handler(OperatorLicenseExpired @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLicenseExpired>();
        _trackerCollection.RemoveEvent<NotPermittedToOperate>();
        _trackerCollection.FailEvent<PermittedToOperate>(); //consider setting this failed instead to ensure a InvalidResultNoData is transmitted back
        _trackerCollection.RemoveEvent<OperatorLackedNeededLicense>(); //would still need the required, I think

        throw new NotImplementedException();
    }

    public void Handler(AttemptToStartVehicleStarted @event)
    {
        if (CorrelationId != CorrelationId) { return; }

        _operatorId = @event.Data.OperatorId;
        _vehicleId = @event.Data.VehicleId;

        _trackerCollection.AddEventTracker<OperatorWasFound>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<VehicleWasFound>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<OperatorNotFound>(false, DomainEventType.Failer);
        _trackerCollection.AddEventTracker<VehicleNotFound>(false, DomainEventType.Failer);

        _commandBus.Dispatch(new FindOperator(_operatorId, CorrelationId, @event.EventId));
        _commandBus.Dispatch(new FindVehicle(_vehicleId, CorrelationId, @event.EventId));

        PublishEventIfPossible();
    }

    public void Handler(VehicleNotRequiredToRemoveOperator @event)
    {
        throw new NotImplementedException();
    }

    public void Handler(VehicleRemovedOperator @event)
    {
        throw new NotImplementedException();
    }
}
