using Common.Events.Domain;
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
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed; //how does this and the one below handle an empty dictionary? Pretty sure on the answer, but check to be sure

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public AlterLicenseTypeProcessManager(IVehicleCommandBus commandBus)
    { //still need to ensure the renew path is complete.
        //in reality it might have been better to split this into 3 pms, one for altered, age requirement, and renew period to help keep it more simpel
        ProcessManagerId = Guid.NewGuid(); //the non altered event pms acting like 'sub' pms
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

        _trackerCollection.UpdateEvent<OperatorLicenseAgeRequirementValidated>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseRetracted>();

        PublishEventIfPossible();
    }

    public void Handler(OperatorLicenseRetracted @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorLicenseRetracted>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorForAgeValidatioNotFound>();
        _trackerCollection.RemoveEvent<OperatorLicenseAgeRequirementValidated>();

        _trackerCollection.AddEventTracker<LicenseTypeOperatorRemoved>(true);
        _trackerCollection.AddEventTracker<FoundVehicleInformations>(true);
        //vehicle does not know the license type only vehicle information does
        //so instead of the code above and the command below, first need to ask for all vehicle informations that use the specific license type
        //then do the above and below, but instead of LicenseTypeId it should be vehicle information id
        //FindVehicleInformationsWithSpecificLicenseType cmd to send to get the vehicle informations
        //have a cmd to get a list of vehicles, the cmd needs the vehicle information ids and the operator id (as it is needed later on) 
        //the hdl can then add an event with the list of vehicles with the specific vehicle information ids and operator id. The event also need the operator id
        //then for each vehicle id, transit the cmd as done above.

        //need an cmd/event to get the vehicle information ids
        //command: FindVehicleInformationsWithSpecificLicenseType 
        //event: FoundVehicleInformations

        //for processing that data
        //cmd FindVehiclesWithSpecificVehicleInformationAndOperator
        //event VehiclesFoundWithSpecificVehicleInformationAndOperator

        _commandBus.Dispatch(new FindVehicleInformationsWithSpecificLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    } 
    

    public void Handler(OperatorLicenseExpired @event)
    { //should an expired license operator be removed from license type? Have a PM that check, before a vehicle is used, if the license is expired or not
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
        if (@event.CorrelationId != CorrelationId) { return; }


        _trackerCollection.UpdateEvent<VehicleRemovedOperator>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<VehicleNotRequiredToRemoveOperator>();

        _trackerCollection.AddEventTracker<OperatorRemovedVehicle>(true);
        _commandBus.Dispatch(new RemoveVehicleFromOperator(@event.Data.VehicleId, @event.Data.OperatorId, @event.CorrelationId, @event.EventId)); //dispatch cmd to remove vehicle from operator

        PublishEventIfPossible();
    }

    public void Handler(VehicleNotRequiredToRemoveOperator @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<VehicleNotRequiredToRemoveOperator>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<VehicleRemovedOperator>();
        PublishEventIfPossible();
    }

    public void Handler(OperatorRemovedVehicle @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorRemovedVehicle>(DomainEventStatus.Completed);
        PublishEventIfPossible();
    }

    public void Handler(VehiclesFoundWithSpecificVehicleInformationAndOperator @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<VehiclesFoundWithSpecificVehicleInformationAndOperator>(DomainEventStatus.Completed);

        foreach (var vehicleId in @event.Data.VehicleIds)
        {
            _trackerCollection.AddEventTracker<VehicleRemovedOperator>(true);
            _trackerCollection.AddEventTracker<VehicleNotRequiredToRemoveOperator>(true);
            _commandBus.Dispatch(new RemoveOperator(@event.Data.OperatorId, vehicleId, @event.CorrelationId, @event.EventId)); //will require a new cmd as the current used for removing an operator does not care about license 
        }
        PublishEventIfPossible();
    }

    public void Handler(FoundVehicleInformations @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<FoundVehicleInformations>(DomainEventStatus.Completed);

        _trackerCollection.AddEventTracker<VehiclesFoundWithSpecificVehicleInformationAndOperator>(true);
        _commandBus.Dispatch(new FindVehiclesWithSpecificVehicleInformationAndOperator(@event.Data.OperatorId, @event.Data.VehicleInformationIds, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }
}
