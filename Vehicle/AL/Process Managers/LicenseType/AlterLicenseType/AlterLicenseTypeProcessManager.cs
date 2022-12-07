using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;

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
        //need to ensure that it cannot return the final event, that it is finished, until all 'paths' are done processing
        //could have enums, "not started", "waiting on", and "done" and then have a variable for each command that is going to be pushed. It goes from not started to waiting and can only go to done when an event is returned
        //if (true) //age requirement changed
        //{

        //}
        //var c1 = new ValidateLicenseAgeRequirementBecauseChange();
        //if (true) //renew period changed
        //{

        //}
        //var c2 = new ValidateLicenseRenewPeriodBecauseChange();
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
        _trackerCollection.AddEventTracker<OperatorLicenseValidated>(true, amount);
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
        _trackerCollection.AddEventTracker<OperatorLicenseValidated>(true, amount);
        _trackerCollection.AddEventTracker<OperatorLicenseExpired>(true, amount);

        foreach(var operatorId in @event.Data.OperatorIds)
        { //need cmd and hdl
            //should send the new age requirement over in case it could end up pulling the old data (not that it could in this scenario)
            //but best not to make assumptions on how the repository gets its data (e.g. entity framework will check its catch first) 
        }

        throw new NotImplementedException();
    }

    public void Handler(OperatorForRenewValidationNotFound @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<OperatorForRenewValidationNotFound>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<OperatorLicenseValidated>();
        _trackerCollection.RemoveEvent<OperatorLicenseExpired>();

        _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.Data.OperatorId, @event.Data.LicenseTypeId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

    public void Handler(OperatorForAgeValidatioNotFound @event)
    {
        throw new NotImplementedException();
    }

    public void Handler(OperatorLicenseValidated @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }

    public void Handler(OperatorLicenseRetracted @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }

    public void Handler(OperatorLicenseExpired @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        throw new NotImplementedException();
    }
}
