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

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }

    public PersonalInformationChangeProcessManager(IPeopleCommandBus commandBus) //not sure if this is the best way to do this, figure out where the instance will be created, e.g. event bus or middlewaren 
    { //could be a good idea to create the instance before running the command, just so it can listen to all events.
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _trackerCollection = new();
        _trackerCollection.AddEvent<PersonChangedGender>(true); //added here because the command handler can add it to the list of event to publish, are reconsider redesigning that part.
        _trackerCollection.AddEvent<PersonPersonalInformationChangedSuccessed>(true);
        _trackerCollection.AddEvent<PersonPersonalInformationChangedFailed>(false);
    }

    public void SetUp(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    //have a method for subscription a service handler to a very special event (finish event)
    //instead of an action it is a Func<TEvent,Result>. So an 'event' that returns data (either SuccessResultNoData or InvalidResult(string[])

    //private Result Finished()
    //{ //figure out how to best run code for next stage and when fully done.
    //    throw new NotImplementedException(); //microsoft states it should return an event, which means the services need to be handle specific events
    //    //could send over handlers for the specific events and then wait on them to have run. After all there is no way for the process manager to directly transmit data back, since all it does is to response to events
    //}

    public void Handler(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Finished);
        if (!@event.Data.GenderWasChanged) 
        {
            _trackerCollection.RemoveEvent<PersonRemovedFromGenderSuccessed>();
        }
    }

    public void Handler(PersonPersonalInformationChangedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Failed);

        _trackerCollection.RemoveEvent<PersonRemovedFromGenderSuccessed>();

        _errors = _errors.Concat(@event.Errors);
    }

    public void Handler(PersonAddedToGenderSuccessed @event)
    {
        if(@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderSuccessed>(DomainEventStatus.Finished); 
        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Finished);
    }

    public void Handler(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonAddedToGenderSuccessed>(DomainEventStatus.Failed);
        _errors = _errors.Concat(@event.Errors);
    }

    public void Handler(PersonRemovedFromGenderSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Finished);
    }

    public void Handler(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Failed);
        _errors = _errors.Concat(@event.Errors);
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
    }

}
