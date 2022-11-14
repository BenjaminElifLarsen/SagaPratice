using Common.RepositoryPattern;
using PeopleDomain.DL.Events.Domain;
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

    public void Save()
    {
        //trigger events and remove them
        var roots = (_genderRepository.AllForOperationsAsync().Result as IEnumerable<IAggregateRoot>).Concat(_personRepository.AllForOperationsAsync().Result as IEnumerable<IAggregateRoot>);
        foreach(var root in roots) //code above does not get the entities not yet saved. Will have to modify mock context and mock repository implementations
        {
            foreach(var @event in root.Evnets)
            { 
                _personEventPublisher.Publish(@event);
            }
        }
        _genderRepository.Save(); //could change it to save over all repository, but it would require to rewrite the context and mock base repository
        _personRepository.Save(); //e.g. moving the collection of unsaved entities over to the context. 
    }
}
