﻿using Common.CQRS.Commands;
using Common.Events.Base;
using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
internal sealed class GenderRecogniseProcessManager : RecogniseProcessManager
{ //need a GenderProcessRouter and repositories for each gender pm 
    //private readonly IPeopleCommandBus _commandBus;
    private readonly List<string> _errors;
    private readonly List<ICommand> _commands;
    private readonly List<IBaseEvent> _events;

    //private readonly HashSet<Action<ProcesserFinished>> _handlers;
    /*
     * ^ collection of handlers cannot be stored in context
     * instead of saving these, these should be used to transmit a response back for when it hits a point where it has to wait,
     * the point of it being stored in the context as the long running business process it is, so waiting on further events that are caused by commands it did not generate.
     * this might not be needed for this specific pm, but for others this could be important.
     * thus, the ProcesserFinished event should be renamed to like StateChanged or something like that and be a system event
     * cosnider if the handlers should subscribe to the pms or if the pms should transmit the event to the event bus
     */
    //update to a new way of keeping state, one that better can be stored in a context
    /*
     * idea: enums used to control state.
     * each handler contain a siwtch case for State that decide what to do.
     * the pm should contains fields/properties for the different values it need, like ids, commands and such
     *  could also be useful for the cases where an handler transmit multiple commands, storing the amount and the different commandIds and then the event handlers can track which causationIds the events have and count the amount they receive
     * 
     * after just eat it require process manager factory (the special router) and message router. So it seems like instead of an event handler, there should be a thing like GenderRecogniseRouter (and so on, for each process manager), which loads in the pm (or creates a new one) and handles the event be sending it to the pm and then saves the pm after it has handled it
     * 
     * 
     * after https://tech.justeattakeaway.com/2015/05/26/process-managers/ commands should be transmitted when the pm is saved and repository should transmit them via the infrastructure
     * so when received an event, run code, save to context, commands are send. Some people state it could be a good idea to save comlmands in the context, but other say it does not matter
     */

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public RecogniseGenderState State { get; private set; }
    public IEnumerable<ICommand> Commands => _commands;
    public IEnumerable<IBaseEvent> Events => _events;

    public GenderRecogniseProcessManager(/*IPeopleCommandBus commandBus*/)
    {
        //_commandBus = commandBus;
        ProcessManagerId = Guid.NewGuid();
        _errors = new();
        _commands = new();
        _events = new();
        //_handlers = new();
        State = RecogniseGenderState.NotStarted;

    }

    public override void Handle(GenderRecognisedSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case RecogniseGenderState.NotStarted:
                State = RecogniseGenderState.GenderRecognised;
                _events.Add(new RecognisedSucceeded(@event.CorrelationId, @event.EventId));
                //create the state change event response thing
                break;

            case RecogniseGenderState.GenderRecognised: // Idempotence.
                break;

            case RecogniseGenderState.GenderFailedToRecognise: //figure out what to do here (not expected to happen really, more the other way around [that is if for some reason the command has run twice)
                break;

            default:
                //log this as default, Unknown, should never be sat and handle the problem. 
                break;
        }

    }

    public override void Handle(GenderRecognisedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case RecogniseGenderState.NotStarted:
                State = RecogniseGenderState.GenderFailedToRecognise;
                _errors.AddRange(@event.Errors);
                _events.Add(new RecognisedFailed(_errors,@event.CorrelationId, @event.CausationId));
                break;

            case RecogniseGenderState.GenderRecognised: // Idempotence.
                break;

            case RecogniseGenderState.GenderFailedToRecognise:
                break;

            default: //log
                break;
        }


    }

    //public void PublishEventIfPossible()
    //{ //not really needed if starting to publish on each state change
       
    //}

    //public void RegistrateHandler(Action<ProcesserFinished> handler)
    //{
    //    _handlers.Add(handler);
    //}

    public void SetUp(Guid correlationId)
    {
        if(CorrelationId == default)
        {
            CorrelationId = correlationId;
        }
    }

    public enum RecogniseGenderState
    {
        NotStarted = 1,
        //Started = 2,
        GenderRecognised = 3,
        GenderFailedToRecognise = 4,

        Unknown = 0,
    }
}