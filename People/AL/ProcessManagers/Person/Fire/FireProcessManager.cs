using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Events.Domain;
using static PersonDomain.AL.ProcessManagers.Person.Fire.FireProcessManager.FirePersonState;

namespace PersonDomain.AL.ProcessManagers.Person.Fire;
public sealed class FireProcessManager : BaseProcessManager, IFireProcessManager
{
    public FirePersonState State { get; private set; }

    public FireProcessManager(Guid correlationId) : base(correlationId)
    {
        ProcessManagerId = Guid.NewGuid();
        State = NotStarted;
    }

    public void Handle(PersonFiredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                State = PersonFired;
                AddCommand(new RemovePersonFromGender(@event.AggregateId, @event.GenderId, @event.CorrelationId, @event.EventId));
                AddStateEvent(new StateEvents.FiredSucceeded(@event.CorrelationId, @event.EventId));
                break;

            case PersonFired:
                break;

            case PersonFailedToBeFired: 
                break;

            case PersonRemovedFromGender: 
                break;

            case PersonFailedToBeRemovedFromGender: 
                break;

            default: break;
        }
    }

    public void Handle(PersonFiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                State = PersonFailedToBeFired;
                AddErrors(@event.Errors);
                AddStateEvent(new StateEvents.FiredFailed(Errors, @event.CorrelationId, @event.EventId));
                break;

            case PersonFired:
                break;

            case PersonFailedToBeFired:
                break;

            case PersonRemovedFromGender:
                break;

            case PersonFailedToBeRemovedFromGender:
                break;

            default: break;
        }
    }

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                break;

            case PersonFired:
                State = PersonRemovedFromGender;
                AddStateEvent(new StateEvents.RemovedFromGenderSucceeded(@event.CorrelationId, @event.EventId));
                break;

            case PersonFailedToBeFired:
                break;

            case PersonRemovedFromGender:
                break;

            case PersonFailedToBeRemovedFromGender:
                break;

            default: break;
        }

        //_trackerCollection.CompleteEvent<PersonRemovedFromGenderSucceeded>();
        //_trackerCollection.RemoveEvent<PersonRemovedFromGenderFailed>();
    }

    public void Handle(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                break;

            case PersonFired:
                State = PersonFailedToBeRemovedFromGender;
                AddErrors(@event.Errors);
                AddStateEvent(new StateEvents.RemovedFromGenderFailed(Errors, @event.CorrelationId, @event.EventId));
                break;

            case PersonFailedToBeFired:
                break;

            case PersonRemovedFromGender:
                break;

            case PersonFailedToBeRemovedFromGender:
                break;

            default: break;
        }
    }

    public enum FirePersonState
    {
        NotStarted = 1,
        PersonFired = 2,
        PersonFailedToBeFired = 3,
        PersonRemovedFromGender = 4,
        PersonFailedToBeRemovedFromGender = 5,

        Unknown = 0
    }
}
