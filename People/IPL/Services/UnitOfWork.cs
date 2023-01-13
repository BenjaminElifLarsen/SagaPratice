using Common.Events.System;
using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Context;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
using PersonDomain.IPL.Repositories.ProcesserManagers;

namespace PersonDomain.IPL.Services;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonDomainEventBus _eventBus;
    private readonly IPersonContext _context;
    private readonly IEnumerable<IProcessManager> _processManagers;
    private readonly IGenderRecogniseProcessRepository _genderRecogniseRepository;
    private readonly IGenderEventRepository _genderEventRepository;
    private readonly IPersonEventRepository _personEventRepository;

    public IGenderRepository GenderRepository => _genderRepository;
    public IPersonRepository PersonRepository => _personRepository;
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository => _genderRecogniseRepository;

    public IGenderEventRepository GenderEventRepository => _genderEventRepository;

    public IPersonEventRepository PersonEventRepository => _personEventRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPersonDomainEventBus eventBus, IPersonContext context, IEnumerable<IProcessManager> processManagers, 
        IGenderRecogniseProcessRepository genderRecogniseRepository, IGenderEventRepository genderEventRepository, IPersonEventRepository personEventRepository)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _eventBus = eventBus;
        _context = context;
        _processManagers = processManagers;
        _genderRecogniseRepository = genderRecogniseRepository;
        _genderEventRepository = genderEventRepository;

        _personEventRepository = personEventRepository;
    }

    private void Save(ProcesserFinished @event) //might not be neeeded with the new design. Will still need to ensure if any part fails that data is not saved.
    { //maybe don't let the command handlers save, save is a handler that listen to a very specific event 'ProcessManagerFinishedSuccessful' or a command called 'ContextSave' (created by the pm) (and that command handler can create the event ContextSaved) or something like that, so like this method, but not using Result, just event type
        if(@event.Result is SuccessResultNoData) //will have to be careful if threading at some point then. In case the service returns before the context has saved.
        { 
            _context.Save(); 
        } 
    } 

    public void Save() 
    {
        var pm = _processManagers.SingleOrDefault(x => x.CorrelationId != default); //old design, not needed with the new design
        if (pm is not null) 
        {
            pm?.RegistrateHandler(Save);
        }
        ProcessEvents();
        if (pm is null) 
        {
            _context.Save();
        }
    }

    public void AddSystemEvent(SystemEvent @event)
    {
        _context.Add(@event);
    }

    public void ProcessEvents()
    {
        do
        {
            var eventsArray = _context.SystemEvents.ToArray();
            foreach (var @event in eventsArray)
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

}
