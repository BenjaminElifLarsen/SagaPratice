using BaseRepository;
using Common.RepositoryPattern;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Model;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.IPL.Services;
internal class UnitOfWork : IUnitOfWork
{
    private readonly IGenderRepository _genderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonEventPublisher _personEventPublisher;

    public IGenderRepository GenderRepository => _genderRepository;

    public IPersonRepository PersonRepository => _personRepository;

    public UnitOfWork(IGenderRepository genderRepository, IPersonRepository personRepository, IPersonEventPublisher personEventPublisher)
    {
        _genderRepository = genderRepository;
        _personRepository = personRepository;
        _personEventPublisher = personEventPublisher;
    }

    public void Save() //does not work by having variables of each context<T> as they are different than those in the repositories
    {
        //would prefer to get the data via the context rather than this.
        var roots = (_genderRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>).Concat(_personRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>).ToArray();
        for(int i = 0; i < roots.Count(); i++)
        { 
            for(int n = 0; n < roots[i].Events.Count(); n++)
            {
                _personEventPublisher.Publish(roots[i].Events.ToArray()[n]);
                roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
            }
        }
        _personRepository.Save(); //this will save for the entire context, change how this is done.
    }
}
