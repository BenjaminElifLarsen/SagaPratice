using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
internal sealed class PersonalInformationChangeProcessManager : IPersonalInformationChangeProcessManager
{
    private readonly IPersonCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public PersonalInformationChangeProcessManager(IPersonCommandBus commandBus) 
    { //could log id
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void SetUp(Guid correlationId)
    { //could log the corelationId together with the ProcessManagerId
        if(CorrelationId == default) {
            CorrelationId = correlationId;
            _trackerCollection.AddEventTracker<PersonChangedGender>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<PersonPersonalInformationChangedSuccessed>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<PersonPersonalInformationChangedFailed>(false, DomainEventType.Failer);
        }
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

    public void Handle(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 

        _trackerCollection.CompleteEvent<PersonPersonalInformationChangedSuccessed>();
        _trackerCollection.RemoveEvent<PersonPersonalInformationChangedFailed>();
        if (!@event.GenderWasChanged) 
        {
            _trackerCollection.RemoveEvent<PersonChangedGender>();
        }
        PublishEventIfPossible();
    }

    public void Handle(PersonPersonalInformationChangedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PersonPersonalInformationChangedFailed>();
        _trackerCollection.FailEvent<PersonPersonalInformationChangedSuccessed>();

        _trackerCollection.RemoveEvent<PersonChangedGender>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        if(@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PersonAddedToGenderSucceeded>(); 
        _trackerCollection.RemoveEvent<PersonAddedToGenderFailed>();
        PublishEventIfPossible();
    }

    public void Handle(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PersonAddedToGenderFailed>();
        _trackerCollection.FailEvent<PersonAddedToGenderSucceeded>();
        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PersonRemovedFromGenderSucceeded>();
        _trackerCollection.RemoveEvent<PersonRemovedFromGenderFailed>();
        PublishEventIfPossible();
    }

    public void Handle(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.CompleteEvent<PersonRemovedFromGenderFailed>();
        _trackerCollection.FailEvent<PersonRemovedFromGenderSucceeded>();
        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handle(PersonChangedGender @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.AddEventTracker<PersonAddedToGenderSucceeded>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<PersonAddedToGenderFailed>(false, DomainEventType.Failer); 
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderSucceeded>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderFailed>(false, DomainEventType.Failer);

        _trackerCollection.CompleteEvent<PersonChangedGender>();

        _commandBus.Dispatch(new AddPersonToGender(@event.AggregateId, @event.NewGenderId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemovePersonFromGender(@event.AggregateId, @event.OldGenderId, @event.CorrelationId, @event.EventId));
        PublishEventIfPossible();
    }

}
