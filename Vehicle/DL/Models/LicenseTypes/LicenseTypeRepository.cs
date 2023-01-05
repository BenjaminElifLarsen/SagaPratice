using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseTypeRepository : ILicenseTypeRepository
{
    private readonly IBaseRepository<LicenseType> _baseRepository;

    public LicenseTypeRepository(IBaseRepository<LicenseType> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public void Create(LicenseType entity)
    {
        _baseRepository.Create(entity);
    }

    public void Delete(LicenseType entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.Id == id, query);
    }

    public async Task<LicenseType> GetForOperationAsync(Guid id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.Id == id);
    }

    public async Task<bool> IsIdUniqueAsync(Guid id)
    {
        return await _baseRepository.IsUniqueAsync(x => x.Id == id);
    }

    public async Task<bool> IsTypeUniqueAsync(string type)
    {
        return await _baseRepository.IsUniqueAsync(x => x.Type == type);
    }

    public void Update(LicenseType entity)
    {
        _baseRepository.Update(entity);
    }
}
