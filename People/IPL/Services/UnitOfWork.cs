using Common.Events.Domain;
using PeopleDomain.AL;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.IPL.Services;
internal class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IDomainEventBus _eventBus;
    private readonly IPeopleContext _context;

    public IGenderRepository GenderRepository => _genderRepository;

    public IPersonRepository PersonRepository => _personRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IDomainEventBus eventBus, IPeopleContext context/*, Registry registry*/)
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
        var roots = _context.GetTracked.ToArray();
        for(int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
        { 
            for(int n = 0; n < roots[i].Events.Count(); n++)
            {
                _eventBus.Publish(roots[i].Events.ToArray()[n]);
                roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
            }
        }
        _context.Save();
    }
}
