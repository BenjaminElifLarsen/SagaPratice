using Common.Events.Domain;
using Common.Events.System;
using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories.DomainModels;
using PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;

namespace PeopleDomain.IPL.Services;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonDomainEventBus _eventBus;
    private readonly IPeopleContext _context;
    private readonly IEnumerable<IProcessManager> _processManagers;
    private readonly IGenderRecogniseProcessRepository _genderRecogniseRepository;

    public IGenderRepository GenderRepository => _genderRepository;
    public IPersonRepository PersonRepository => _personRepository;
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository => _genderRecogniseRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPersonDomainEventBus eventBus, IPeopleContext context, IEnumerable<IProcessManager> processManagers, IGenderRecogniseProcessRepository genderRecogniseRepository)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _eventBus = eventBus;
        _context = context;
        _processManagers = processManagers;
        _genderRecogniseRepository = genderRecogniseRepository;
    }

    private void Save(ProcesserFinished @event)
    {
        if(@event.Result is SuccessResultNoData)
        { //this will be called first when all events have been run. Could place the ProcesserFinished event on the evnet publisher or have a while check down in Save() that check if the pm is finished and then places it
            _context.Save(); //only the pm will know if it has finished, unit of work should not be one to tell pm that is has finished.
        } //cannot place ProcessorFinished on the event bus as it does not implement IDomainEvent and it has nothing to do with the domain models
    } //service event mayhaps or whatever they were called again (system events?), look into it.

    public void Save() 
    {
        var pm = _processManagers.SingleOrDefault(x => x.CorrelationId != default); //this will require refactorisation to the new design
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
                    roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
                }
            }
        } while (_context.GetTracked.SelectMany(x => x.Events).Any());
        //do
        //{
        //    var events = _context.Events;
        //    foreach (var @event in events) //need a way to remove events or have a variable that state if they have been published.
        //    {
        //        _eventBus.Publish(@event); //the bus should set them to have been published, should events really know if they have published?0
        //    }
        //} while (_context.Events.Any());
        //while (pm.Running) ;
    }

}
