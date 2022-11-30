using Common.Events.Domain;
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

    public IGenderRepository GenderRepository => _genderRepository;

    public IPersonRepository PersonRepository => _personRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPeopleDomainEventBus eventBus, IPeopleContext context/*, Registry registry*/)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _eventBus = eventBus;
        _context = context;
    }

    public void Save() //does not work by having variables of each context<T> as they are different than those in the repositories
    {
        //would prefer to get the data via the context rather than this. Mayhaps have a main IContext file with save and AllTracked {get;}. The problem with that is that it would not be part of the Common module.
        //would require a lot of rework for something that is not an important training part of this software.
        do
        {
            var roots = _context.GetTracked.ToArray();
            for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
            {
                for (int n = 0; n < roots[i].Events.Count(); n++) //does not work correctly as n goes up and event count goes down
                {
                    _eventBus.Publish(roots[i].Events.ToArray()[n]);
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
        _context.Save();
    }
}
