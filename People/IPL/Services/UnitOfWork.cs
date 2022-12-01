using Common.Events.Domain;
using Common.ProcessManager;
using PeopleDomain.AL;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.IPL.Services;
internal class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPeopleDomainEventBus _eventBus;
    private readonly IPeopleContext _context;
    private readonly IEnumerable<IProcessManager> _processManagers;

    public IGenderRepository GenderRepository => _genderRepository;

    public IPersonRepository PersonRepository => _personRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPeopleDomainEventBus eventBus, IPeopleContext context, IEnumerable<IProcessManager> processManagers)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _eventBus = eventBus;
        _context = context;
        _processManagers = processManagers;
    }

    public void Save() 
    {
        do
        {
            var roots = _context.GetTracked.ToArray();
            for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
            {
                for (int n = 0; n < roots[i].Events.Count(); n++) //does not work correctly as n goes up and event count goes down
                {
                    _eventBus.Publish(roots[i].Events.ToArray()[n]); //the add and update method in the repository should add them to the event store
                    roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
                }
            }
        } while (_context.Events.Any());
        //do
        //{
        //    var events = _context.Events;
        //    foreach (var @event in events) //need a way to remove events or have a variable that state if they have been published.
        //    {
        //        _eventBus.Publish(@event); //the bus should set them to have been published, should events really know if they have published?0
        //    }
        //} while (_context.Events.Any());
        var pm = _processManagers.SingleOrDefault(x => x.CorrelationId != default);
        while (pm.Running) ; //consider if this can be done better
        if (pm.FinishedSuccessful)
        {
            _context.Save();
        }
    }
}
