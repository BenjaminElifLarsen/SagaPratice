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

    public PersonalInformationChangeProcessManager(Guid correlationId, IPeopleCommandBus commandBus) //not sure if this is the best way to do this, figure out where the instance will be created, e.g. event bus or middlewaren 
    { //could be a good idea to create the instance before running the command, just so it can listen to all events.
        ProcessManagerId = Guid.NewGuid();
        CorrelationId = correlationId;
        _commandBus = commandBus;
        _trackerCollection = new();
        _trackerCollection.AddEvent<PersonChangedGender>(true); //added here because the command handler can add it to the list of event to publish, are reconsider redesigning that part.
        _trackerCollection.AddEvent<PersonPersonalInformationChangedSuccessed>(true);
        _trackerCollection.AddEvent<PersonPersonalInformationChangedFailed>(false);
    }

    private Result Finished()
    { //figure out how to best run code for next stage and when fully done.
        throw new NotImplementedException(); //microsoft statae it should return an event, which means the services need to be handle specific events
        //could send over handlers for the specific events and then wait on them to have run
    }

    public void Handler(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 
        //should this one trigger the other event (PersonChangedGender) (regarding adding and removing gender)
        //feel like it would make more sense when looking through this file's code. So it is the processer that can trigger events that causes commands, while commands can only trigger events for status (regarding failer or not)
        //the first command would have to publish an event with data rather than status. Also process manager publish an event that it is listning too?
        //consider asking supervisor if they got an idea.
        //if having to publish events got to figure out a way to do it. Currently it can only publish commands and currently events require data from an aggregate root.
        //some information is present in @event but it might not be the needed and also the ctor requires the aggregate root
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedSuccessed>(DomainEventStatus.Finished);
        _trackerCollection.UpdateEvent<PersonPersonalInformationChangedFailed>(DomainEventStatus.Failed);
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
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Finished); //maybe just remove failers instead of setting them to finished
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

        _trackerCollection.UpdateEvent<PersonChangedGender>(DomainEventStatus.Finished);

        _trackerCollection.AddEvent<PersonAddedToGenderSuccessed>(true);
        _trackerCollection.AddEvent<PersonAddedToGenderFailed>(false); 
        _trackerCollection.AddEvent<PersonRemovedFromGenderSuccessed>(true);
        _trackerCollection.AddEvent<PersonRemovedFromGenderFailed>(false);

        _commandBus.Publish(new AddPersonToGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.CorrelationId, @event.EventId));
        _commandBus.Publish(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.OldGenderId, @event.CorrelationId, @event.EventId));
    }
}
