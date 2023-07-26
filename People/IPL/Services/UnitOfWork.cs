using Common.Events.Save;
using Common.Events.System;
using Common.UnitOfWork;
using PersonDomain.AL.Busses.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Context;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
using PersonDomain.IPL.Repositories.ProcesserManagers.Genders;

namespace PersonDomain.IPL.Services;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonDomainEventBus _eventBus;
    private readonly IPersonContext _context;
    private readonly IGenderRecogniseProcessRepository _genderRecogniseRepository;
    private readonly IGenderEventRepository _genderEventRepository;
    private readonly IPersonEventRepository _personEventRepository;

    public IGenderRepository GenderRepository => _genderRepository;
    public IPersonRepository PersonRepository => _personRepository;
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository => _genderRecogniseRepository;

    public IGenderEventRepository GenderEventRepository => _genderEventRepository;

    public IPersonEventRepository PersonEventRepository => _personEventRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPersonDomainEventBus eventBus, IPersonContext context, 
        IGenderRecogniseProcessRepository genderRecogniseRepository, IGenderEventRepository genderEventRepository, IPersonEventRepository personEventRepository)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _eventBus = eventBus;
        _context = context;
        _genderRecogniseRepository = genderRecogniseRepository;
        _genderEventRepository = genderEventRepository;

        _personEventRepository = personEventRepository;
    }

    public void Save() 
    {
        ProcessEvents(); //how does the system handle if one command succeed, but a follow-up commands fail? E.g. firing a person, but they cannot be removed from gender. Test this by modifying the remove person from gender command handler to always return a fail event
        //if (pm is null) //the old design checked the result type, but this does not have that. Maybe a property in StateEvent and some code checks if any stateEvent is a failer.
        //{ //have a boolean in the base StateEvent that indicate if the state event is a success or failer and subscribe to the base... which will not work as the bus pushes out the concrete type and cannot do it on base types, so it would require to have a handle for each implementation.
        _context.Save(); //could let the ProcessEvents check if the current event is a state event and then the boolean
        //} //or have a special 'save' event that process managers can trigger or... don't have IUnitOfWork.Save() calls in the command handlers at all, rather the pm, when done, transmit either a do save event or do not save event that the unit of work is subscribed too
    } //after all the pm event routers also pushes out events. Hmmm... maybe it should be a command, since it is something the unit of work should do. It could then trigger an event called ProcessingFinished(Succeeded/Failed) that the services can be subscribed too.
    //the events added via the command handlers are first published when ProcessEvents are called via the UnitOfWork.Save(), maybe have the routers publish an event that causes processing?
    //remember, processs managers are triggered by the events created by the command handlers and caught by the routers, so need to ensure events are processed someway via the command handlers.
    //could make command handlers call the UnitOfWork.ProcessEvents(), however, there is an inconsistency as not all command handlers should call it, only those which are activated by the original command, not those reacting on commands created by events.
    //just not glad for that kind of inconsistiency.
    //consider solving the dislike toward domain events being stored in the domain models regarding the purity. Some, like Microsoft and a Jimmy Bogard, states to store them in domain model and trigger before saving changes and some say to publish immediately and not store them.
    //when processing the domain events their effects will begin, more commands and more events until there are no more.
    //also both the process manager and the domain model contains events. Domain models only contain domain events and the process managers only contain system events. The domain events created via the system events are always processed via the unit of work event processing.
    //however, some system events are processed by the unit of work (and other system events are published by the router), that is events that indicate a failer which does not create/modify an entity. On that note should unit of work really store system events? Unit of Work is for context operations via the repositories
    //
    //the thing to do first is to update the person process managers to use the new events.
    //and make the command handlers call the processs events rather than the save method, so remove the IBaseUnitOfWork from IUnitOfWork.
    //then solve the entire mess of system events in the unit of work and where else to place them (like a SystemEventContext or something like that). After all the unit of work is for context operations and system events don't belong in the context as they should not be stored and compared to domain events they are not part of a domain model in any way.



    public void AddSystemEvent(SystemEvent @event)
    {
        _context.Add(@event);
    }

    public void ProcessEvents() //should not be called from Save() and should return a bool
    {
        do
        {
            var systemEvents = _context.SystemEvents.ToArray();
            foreach (var @event in systemEvents)
            {
                _eventBus.Publish(@event);
                _context.Remove(@event);
            }
            var roots = _context.GetTracked.ToArray();
            for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
            {
                for (int n = 0; n < roots[i].Events.Count(); n++) //does not work correctly as n goes up and event count goes down
                {
                    _eventBus.Publish(roots[i].Events.ToArray()[n]); //the add and update method in the repository should add them to the event store
                    //add to the context in the event store before removing them, but first save them when the unit of work saves (or should they be saved when the pm is saved
                    //roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);

                }
                if (roots[i] is Gender g)
                {//dont like the none of the implementation of this method at all
                    _genderEventRepository.AddEvents(g);
                }
                if (roots[i] is Person p)
                {
                    _personEventRepository.AddEvents(p);
                }
                for ( int n = roots[i].Events.Count() -1; n >= 0; n--)
                {
                    roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
                }
            }
        } while (_context.GetTracked.SelectMany(x => x.Events).Any());
    }

    public void Handle(SaveProcessedWork command)
    {
        //call save
        _eventBus.Publish(new ProcessingSucceeded(command.CorrelationId, command.CommandId));
    }

    public void Handle(DiscardProcesssedWork command)
    {
        //'restore' (would be so more simpler with EntityFramework Core
        _eventBus.Publish(new ProcessingFailed(command.Errors, command.CorrelationId, command.CommandId));
    }
}
