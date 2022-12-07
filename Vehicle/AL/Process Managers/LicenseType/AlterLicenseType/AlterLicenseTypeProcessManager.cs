using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal class AlterLicenseTypeProcessManager : IAlterLicenseTypeProcessManager
{ 
    private readonly IVehicleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed; //how does this and the one below handle an empty dictionary? Pretty sure on the answer, but check to be sure

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public AlterLicenseTypeProcessManager(IVehicleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
        _trackerCollection.AddEventTracker<LicenseTypeAlteredSuccessed>(true);
        _trackerCollection.AddEventTracker<LicenseTypeAlteredFailed>(false);
        _trackerCollection.AddEventTracker<LicenseTypeAgeRequirementChanged>(true);
        _trackerCollection.AddEventTracker<LicenseTypeRenewPeriodChanged>(true);
    }

    public void SetUp(Guid correlationId)
    {
        CorrelationId = correlationId;
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

    public void Handler(LicenseTypeAlteredSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<LicenseTypeAlteredSuccessed>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<LicenseTypeAlteredFailed>();

        if (!@event.Data.RenewPeriodChanged)
        {
            _trackerCollection.RemoveEvent<LicenseTypeRenewPeriodChanged>();
        }
        if (!@event.Data.AgeRequirementChanged)
        {
            _trackerCollection.RemoveEvent<LicenseTypeAgeRequirementChanged>();
        }
        PublishEventIfPossible();
    }

    public void Handler(LicenseTypeAlteredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<LicenseTypeAlteredSuccessed>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<LicenseTypeAlteredFailed>(DomainEventStatus.Completed);

        _trackerCollection.RemoveEvent<LicenseTypeAgeRequirementChanged>();
        _trackerCollection.RemoveEvent<LicenseTypeRenewPeriodChanged>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(LicenseTypeAgeRequirementChanged @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<LicenseTypeAgeRequirementChanged>(DomainEventStatus.Completed);

        var amount = @event.Data.OperatorIds.Count();
        _trackerCollection.AddEventTracker<OperatorForAgeValidatioNotFound>(true, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseAgeRequirementValidated>(true, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseRetracted>(true, amount);

        foreach(var operatorId in @event.Data.OperatorIds)
        {
            _commandBus.Dispatch(new LicenseAgeRequirementRequireValidation(operatorId, @event.Data.Id, @event.Data.NewAgeRequirement, @event.CorrelationId, @event.EventId));
        }
        PublishEventIfPossible();
    }

    public void Handler(LicenseTypeRenewPeriodChanged @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<LicenseTypeRenewPeriodChanged>(DomainEventStatus.Completed);

        var amount = @event.Data.OperatorIds.Count();
        _trackerCollection.AddEventTracker<OperatorForRenewValidationNotFound>(true, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseRenewPeriodValidated>(true, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseExpired>(true, amount);

        foreach(var operatorId in @event.Data.OperatorIds)
        { //need cmd and hdl
            //should send the new age requirement over in case it could end up pulling the old data (not that it could in this scenario)
            //but best not to make assumptions on how the repository gets its data (e.g. entity framework will check its catch first) 
            _commandBus.Dispatch(new LicenseRenewPeriodRequireValidation(operatorId, @event.Data.Id, @event.Data.NewRenewPeriodInYears, @event.CorrelationId, @event.EventId));
        }
        PublishEventIfPossible();
    }

    public void Handler(OperatorForRenewValidationNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorForRenewValidationNotFound>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorLicenseRenewPeriodValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true);
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

    public void Handler(OperatorForAgeValidatioNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorForAgeValidatioNotFound>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorLicenseAgeRequirementValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseRetracted>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true);
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible(); 
    }

    public void Handler(OperatorLicenseAgeRequirementValidated @event)
    { 
        if (@event.CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }

    public void Handler(OperatorLicenseRetracted @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorLicenseRetracted>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseAgeRequirementValidated>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true);

        foreach(var vehicleId in @event.Data.VehicleIds)
        {
            _trackerCollection.AddEventTracker<VehicleRemovedOperator>(true);
            _trackerCollection.AddEventTracker<VehicleNotRequiredToRemoveOperator>(true);
            _commandBus.Dispatch(new RemoveOperatorIfSpecificLicenseType(@event.Data.OperatorId, vehicleId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId)); //will require a new cmd as the current used for removing an operator does not care about license 
        } 

        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible(); //need to remove them from vehicle and remove vehicles from them
    } //operator knows of vehicles, but which require which license type and the vehicle knows which operator is on it.
    //so could send out commands (one for each vehicle id) to identify vehicles license type id (so OperatorLicenseRetracted require vehicle ids)
    //the cmd can return either VehicleRemovedOperator (the cmd removes the operator and transmit its id back) 
    //or VehicleNotRequiredToRemoveOperator (consider a much better name) without any extra data.
    //for each VehicleRemovedOperator a cmd is send to the operator to remove that specific vehicle id 
    //new RemoveOperatorFromVehicle() can be used for removing the operator when coming to that point
    

    public void Handler(OperatorLicenseExpired @event)
    { //should an expired license operator be removed from license type?
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorLicenseExpired>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorForRenewValidationNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseRenewPeriodValidated>();

        PublishEventIfPossible();
    }

    public void Handler(LicenseTypeOperatorRemoved @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<LicenseTypeOperatorRemoved>(DomainEventStatus.Completed);
        PublishEventIfPossible();
    }

    public void Handler(OperatorLicenseRenewPeriodValidated @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorLicenseRenewPeriodValidated>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();
        _trackerCollection.RemoveEvent<OperatorForRenewValidationNotFound>();

        PublishEventIfPossible();
    }

    public void Handler(VehicleRemovedOperator @event)
    {



        _trackerCollection.UpdateEvent<VehicleRemovedOperator>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<VehicleNotRequiredToRemoveOperator>();


        throw new NotImplementedException();
    }

    public void Handler(VehicleNotRequiredToRemoveOperator @event)
    {


        _trackerCollection.UpdateEvent<VehicleNotRequiredToRemoveOperator>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<VehicleRemovedOperator>();


        throw new NotImplementedException();
    }
}
