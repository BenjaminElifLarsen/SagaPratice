using Common.CQRS.Queries;
using Common.RepositoryPattern;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.DomainModels;
internal sealed class PersonRepository : IPersonRepository
{
    private readonly IBaseRepository<Person> _baseRepository;

    public PersonRepository(IBaseRepository<Person> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public async Task<bool> DoesPersonExist(Guid id)
    {
        return !await _baseRepository.IsUniqueAsync(x => x == id);
    }

    public void Fire(Person entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x == id, query);
    }

    public async Task<Person> GetForOperationAsync(Guid id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x == id);
    }

    public void Hire(Person entity)
    {
        _baseRepository.Create(entity);
    }

    public void UpdatePersonalInformation(Person entity)
    {
        _baseRepository.Update(entity);
    }
}
