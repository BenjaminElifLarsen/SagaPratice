using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
internal class PersonalInformationChangeProcessManager : IPersonalInformationChangeProcessManager
{
    private IPeopleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private IEnumerable<string> _errors;
    private Action<Result> _callback;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }

    public bool Running => !_trackerCollection.AllFinishedOrFailed;

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public PersonalInformationChangeProcessManager(IPeopleCommandBus commandBus) 
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _errors = new List<string>();
        _trackerCollection = new();
        _trackerCollection.AddEvent<PersonChangedGender>(true);
        _trackerCollection.AddEvent<PersonPersonalInformationChangedSuccessed>(true);
        _trackerCollection.AddEvent<PersonPersonalInformationChangedFailed>(false);
    }

    public void SetUp(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public void SetCallback(Action<Result> callback)
    {
        _callback = callback;
    }
    //consider a better way than these two
    public void RunCallbackIfPossible()
    {
        if (_trackerCollection.AllFinishedOrFailed)
        {
            Result result = !_trackerCollection.Failed ? new SuccessResultNoData() : new InvalidResultNoData(_errors.ToArray());
            _callback.Invoke(result);
        }
    }

    public void Handler(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Finished);
        if (!@event.Data.GenderWasChanged) 
        {
            _trackerCollection.RemoveEvent<PersonChangedGender>();
        }
        RunCallbackIfPossible();
    }

    public void Handler(PersonPersonalInformationChangedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Failed);

        _trackerCollection.RemoveEvent<PersonChangedGender>();

        _errors = _errors.Concat(@event.Errors);
        RunCallbackIfPossible();
    }

    public void Handler(PersonAddedToGenderSuccessed @event)
    {
        if(@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderSuccessed>(DomainEventStatus.Finished); 
        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Finished);
        RunCallbackIfPossible();
    }

    public void Handler(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonAddedToGenderSuccessed>(DomainEventStatus.Failed);
        _errors = _errors.Concat(@event.Errors);
        RunCallbackIfPossible();
    }

    public void Handler(PersonRemovedFromGenderSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Finished);
        RunCallbackIfPossible();
    }

    public void Handler(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Failed);
        _errors = _errors.Concat(@event.Errors);
        RunCallbackIfPossible();
    }

    public void Handler(PersonChangedGender @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.AddEvent<PersonAddedToGenderSuccessed>(true);
        _trackerCollection.AddEvent<PersonAddedToGenderFailed>(false); 
        _trackerCollection.AddEvent<PersonRemovedFromGenderSuccessed>(true);
        _trackerCollection.AddEvent<PersonRemovedFromGenderFailed>(false);

        _trackerCollection.UpdateEvent<PersonChangedGender>(DomainEventStatus.Finished);

        _commandBus.Dispatch(new AddPersonToGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.OldGenderId, @event.CorrelationId, @event.EventId));
        RunCallbackIfPossible();
    }

}
