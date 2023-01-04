using Common.CQRS.Queries;
using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.IPL.Repositories.DomainModels;
internal sealed class GenderRepository : IGenderRepository
{
    private readonly IBaseRepository<Gender> _baseRepository;

    public GenderRepository(IBaseRepository<Gender> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public void Recognise(Gender entity)
    {
        _baseRepository.Create(entity);
    }

    public void Unrecognise(Gender entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x == id, query);
    }

    public void Update(Gender entity)
    {
        _baseRepository.Update(entity);
    }

    public async Task<Gender> GetForOperationAsync(int id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x == id);
    }

}
