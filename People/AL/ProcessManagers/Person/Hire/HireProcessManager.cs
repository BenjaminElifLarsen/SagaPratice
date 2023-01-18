using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Events.Domain;
using static PersonDomain.AL.ProcessManagers.Person.Hire.HireProcessManager.HirePersonState;

namespace PersonDomain.AL.ProcessManagers.Person.Hire;
public sealed class HireProcessManager : BaseProcessManager, IHireProcessManager
{
    public HirePersonState State { get; private set; }

    public HireProcessManager(Guid correlationId) : base(correlationId)
    {
        ProcessManagerId = Guid.NewGuid();
        State = NotStarted;
    }

    public void Handle(PersonHiredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                State = PersonHired;
                AddCommand(new AddPersonToGender(@event.AggregateId, @event.GenderId, @event.CorrelationId, @event.EventId));
                AddStateEvent(new HiredSucceeded(@event.CorrelationId, @event.EventId));
                break;

            case PersonHired: 
                break;

            case PersonFailedToBeHired: 
                break;

            case PersonAddedToGender: 
                break;

            case PersonFailedToBeAddedToGender: 
                break;

            default: break;
        }
    }

    public void Handle(PersonHiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                State = PersonFailedToBeHired;
                AddErrors(@event.Errors);
                AddStateEvent(new HiredFailed(Errors, @event.CorrelationId, @event.EventId));
                break;

            case PersonHired:
                break;

            case PersonFailedToBeHired:
                break;

            case PersonAddedToGender:
                break;

            case PersonFailedToBeAddedToGender:
                break;

            default: break;
        }
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                break;

            case PersonHired:
                State = PersonAddedToGender;
                AddStateEvent(new AddedToGenderSucceeded(@event.CorrelationId, @event.EventId));
                break;

            case PersonFailedToBeHired:
                break;

            case PersonAddedToGender:
                break;

            case PersonFailedToBeAddedToGender:
                break;

            default: break;
        }
    }

    public void Handle(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                break;

            case PersonHired:
                State = PersonFailedToBeAddedToGender;
                AddErrors(@event.Errors);
                AddStateEvent(new AddedToGenderFailed(Errors, @event.CorrelationId, @event.EventId));
                break;

            case PersonFailedToBeHired:
                break;

            case PersonAddedToGender:
                break;

            case PersonFailedToBeAddedToGender:
                break;

            default: break;
        }
    }

    public enum HirePersonState
    {
        NotStarted = 1,
        PersonHired = 2,
        PersonFailedToBeHired = 3,
        PersonAddedToGender = 4,
        PersonFailedToBeAddedToGender = 5,

        Unknown = 0
    }
}
