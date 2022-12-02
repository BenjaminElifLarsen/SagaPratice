using Common.CQRS.Queries;
using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.IPL.Repositories;
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

    public async Task<bool> DoesPersonExist(int id)
    {
        return !await _baseRepository.IsUniqueAsync(x => x == id);
    }

    public void Fire(Person entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x == id, query);
    }

    public async Task<Person> GetForOperationAsync(int id)
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
