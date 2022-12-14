using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
internal sealed class PersonalInformationChangeProcessManager : IPersonalInformationChangeProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public PersonalInformationChangeProcessManager(IPeopleCommandBus commandBus) 
    { //could log id
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
        _trackerCollection.AddEventTracker<PersonChangedGender>(true);
        _trackerCollection.AddEventTracker<PersonPersonalInformationChangedSuccessed>(true);
        _trackerCollection.AddEventTracker<PersonPersonalInformationChangedFailed>(false);
    }

    public void SetUp(Guid correlationId)
    { //could log the corelationId together with the ProcessManagerId
        CorrelationId = correlationId;
    }

    public void RegistrateHandler(Action<ProcesserFinished> handler)
    {
        _handlers.Add(handler); //improve this to ensure the same instance of a method cannot be registrated multiple times
    }

    public void PublishEventIfPossible() 
    { //not to happy with this one, name and implementation, feel like it could be done better
        if (_trackerCollection.AllFinishedOrFailed)
        {
            Result result = !_trackerCollection.Failed ? new SuccessResultNoData() : new InvalidResultNoData(_errors.ToArray());
            ProcesserFinished @event = new(result, ProcessManagerId);
            foreach(var handler in _handlers)
            {
                handler.Invoke(@event);
            }
        }
    }

    public void Handler(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonPersonalInformationChangedFailed>();
        if (!@event.Data.GenderWasChanged) 
        {
            _trackerCollection.RemoveEvent<PersonChangedGender>();
        }
        PublishEventIfPossible();
    }

    public void Handler(PersonPersonalInformationChangedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Completed);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Failed);

        _trackerCollection.RemoveEvent<PersonChangedGender>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonAddedToGenderSucceeded @event)
    {
        if(@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderSucceeded>(DomainEventStatus.Completed); 
        _trackerCollection.RemoveEvent<PersonAddedToGenderFailed>();
        PublishEventIfPossible();
    }

    public void Handler(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Completed);
        _trackerCollection.UpdateEvent<PersonAddedToGenderSucceeded>(DomainEventStatus.Failed);
        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSucceeded>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonRemovedFromGenderFailed>();
        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Completed);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSucceeded>(DomainEventStatus.Failed);
        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonChangedGender @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.AddEventTracker<PersonAddedToGenderSucceeded>(true);
        _trackerCollection.AddEventTracker<PersonAddedToGenderFailed>(false); 
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderSucceeded>(true);
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderFailed>(false);

        _trackerCollection.UpdateEvent<PersonChangedGender>(DomainEventStatus.Completed);

        _commandBus.Dispatch(new AddPersonToGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.OldGenderId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

}
