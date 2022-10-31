using Common.CQRS.Queries;
using Common.RepositoryPattern;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
internal class GenderRepository : IGenderRepository
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

    public void Save()
    {
        _baseRepository.SaveChanges();
    }
}
