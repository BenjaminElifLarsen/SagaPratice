using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal class AlterLicenseTypeProcessManager : IAlterLicenseTypeProcessManager
{ 
    private readonly IVehicleCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed; //how does this and the one below handle an empty dictionary? Pretty sure on the answer, but check to be sure

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public AlterLicenseTypeProcessManager(IVehicleCommandBus commandBus)
    {
        //in reality it might have been better to split this into 3 pms, one for altered, age requirement, and renew period to help keep it more simpel
        ProcessManagerId = Guid.NewGuid(); //the non altered event pms acting like 'sub' pms
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void SetUp(Guid correlationId)
    {
        if(CorrelationId == default)
        {
            CorrelationId = correlationId;
            _trackerCollection.AddEventTracker<LicenseTypeAlteredSucceeded>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<LicenseTypeAlteredFailed>(false, DomainEventType.Failer);
            _trackerCollection.AddEventTracker<LicenseTypeAgeRequirementChanged>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<LicenseTypeRenewPeriodChanged>(true, DomainEventType.Failer);
        }
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

    public void Handle(LicenseTypeAlteredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<LicenseTypeAlteredSucceeded>();
        _trackerCollection.RemoveEvent<LicenseTypeAlteredFailed>();

        if (!@event.RenewPeriodChanged)
        {
            _trackerCollection.RemoveEvent<LicenseTypeRenewPeriodChanged>();
        }
        if (!@event.AgeRequirementChanged)
        {
            _trackerCollection.RemoveEvent<LicenseTypeAgeRequirementChanged>();
        }
        PublishEventIfPossible();
    }

    public void Handle(LicenseTypeAlteredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.FailEvent<LicenseTypeAlteredSucceeded>();
        _trackerCollection.CompleteEvent<LicenseTypeAlteredFailed>();

        _trackerCollection.RemoveEvent<LicenseTypeAgeRequirementChanged>();
        _trackerCollection.RemoveEvent<LicenseTypeRenewPeriodChanged>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handle(LicenseTypeAgeRequirementChanged @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<LicenseTypeAgeRequirementChanged>();

        var amount = @event.OperatorIds.Count();
        _trackerCollection.AddEventTracker<OperatorForAgeValidatioNotFound>(true, DomainEventType.Succeeder, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseAgeRequirementValidated>(true, DomainEventType.Succeeder, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseRetracted>(true, DomainEventType.Succeeder, amount);

        foreach(var operatorId in @event.OperatorIds)
        {
            _commandBus.Dispatch(new LicenseAgeRequirementRequireValidation(operatorId, @event.AggregateId, @event.NewAgeRequirement, @event.CorrelationId, @event.EventId));
        }
        PublishEventIfPossible();
    }

    public void Handle(LicenseTypeRenewPeriodChanged @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<LicenseTypeRenewPeriodChanged>();

        var amount = @event.OperatorIds.Count();
        _trackerCollection.AddEventTracker<OperatorForRenewValidationNotFound>(true, DomainEventType.Succeeder, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseRenewPeriodValidated>(true, DomainEventType.Succeeder, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseExpired>(true, DomainEventType.Succeeder, amount);

        foreach(var operatorId in @event.OperatorIds)
        {
            _commandBus.Dispatch(new LicenseRenewPeriodRequireValidation(operatorId, @event.AggregateId, @event.NewRenewPeriodInYears, @event.CorrelationId, @event.EventId));
        }
        PublishEventIfPossible();
    }

    public void Handle(OperatorForRenewValidationNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorForRenewValidationNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseRenewPeriodValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true, DomainEventType.Succeeder);
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.OperatorId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

    public void Handle(OperatorForAgeValidatioNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseAgeRequirementValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseRetracted>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true, DomainEventType.Succeeder);
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.OperatorId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible(); 
    }

    public void Handle(OperatorLicenseAgeRequirementValidated @event)
    { 
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLicenseAgeRequirementValidated>();
        _trackerCollection.RemoveEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseRetracted>();

        PublishEventIfPossible();
    }

    public void Handle(OperatorLicenseRetracted @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLicenseRetracted>();
        _trackerCollection.RemoveEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseAgeRequirementValidated>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<FoundVehicleInformations>(true, DomainEventType.Succeeder);

        _commandBus.Dispatch(new FindVehicleInformationsWithSpecificLicenseType(@event.AggregateId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.AggregateId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    } 
    
    public void Handle(OperatorLicenseExpired @event)
    { //should an expired license operator be removed from license type? Have a PM that check, before a vehicle is used, if the license is expired or not
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLicenseExpired>();
        _trackerCollection.RemoveEvent<OperatorForRenewValidationNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseRenewPeriodValidated>();

        PublishEventIfPossible();
    }

    public void Handle(LicenseTypeOperatorRemoved @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<LicenseTypeOperatorRemoved>();
        PublishEventIfPossible();
    }

    public void Handle(OperatorLicenseRenewPeriodValidated @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorLicenseRenewPeriodValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();
        _trackerCollection.RemoveEvent<OperatorForRenewValidationNotFound>();

        PublishEventIfPossible();
    }

    public void Handle(VehicleRemovedOperator @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }


        _trackerCollection.CompleteEvent<VehicleRemovedOperator>();
        _trackerCollection.RemoveEvent<VehicleNotRequiredToRemoveOperator>();

        _trackerCollection.AddEventTracker<OperatorRemovedVehicle>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<OperatorNotFound>(false, DomainEventType.Succeeder);
        _commandBus.Dispatch(new RemoveVehicleFromOperator(@event.AggregateId, @event.OperatorId, @event.CorrelationId, @event.EventId)); //dispatch cmd to remove vehicle from operator

        PublishEventIfPossible();
    }

    public void Handle(VehicleNotRequiredToRemoveOperator @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<VehicleNotRequiredToRemoveOperator>();
        _trackerCollection.RemoveEvent<VehicleRemovedOperator>();
        PublishEventIfPossible();
    }

    public void Handle(OperatorRemovedVehicle @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorRemovedVehicle>();
        _trackerCollection.RemoveEvent<OperatorNotFound>();
        PublishEventIfPossible();
    }

    public void Handle(VehiclesFoundWithSpecificVehicleInformationAndOperator @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<VehiclesFoundWithSpecificVehicleInformationAndOperator>();

        foreach (var vehicleId in @event.VehicleIds)
        {
            _trackerCollection.AddEventTracker<VehicleRemovedOperator>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<VehicleNotRequiredToRemoveOperator>(true, DomainEventType.Succeeder);
            _commandBus.Dispatch(new RemoveOperatorFromVehicle(vehicleId, @event.OperatorId, @event.CorrelationId, @event.EventId)); //will require a new cmd as the current used for removing an operator does not care about license 
        }
        PublishEventIfPossible();
    }

    public void Handle(FoundVehicleInformations @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<FoundVehicleInformations>();

        _trackerCollection.AddEventTracker<VehiclesFoundWithSpecificVehicleInformationAndOperator>(true, DomainEventType.Succeeder);
        _commandBus.Dispatch(new FindVehiclesWithSpecificVehicleInformationAndOperator(@event.OperatorId, @event.VehicleInformationIds, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

    public void Handle(OperatorNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<OperatorNotFound>();
        _trackerCollection.RemoveEvent<OperatorRemovedVehicle>();
        PublishEventIfPossible();
    }
}
