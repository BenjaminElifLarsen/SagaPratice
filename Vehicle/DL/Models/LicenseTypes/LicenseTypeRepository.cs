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

    public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.LicenseTypeId == id, query);
    }

    public async Task<LicenseType> GetForOperationAsync(int id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.LicenseTypeId == id);
    }

    public async Task<bool> IsIdUniqueAsync(int id)
    {
        return await _baseRepository.IsUniqueAsync(x => x.LicenseTypeId == id);
    }

    public async Task<bool> IsTypeUniqueAsync(string type)
    {
        return await _baseRepository.IsUniqueAsync(x => x.Type == type);
    }

    public void Save()
    {
        throw new NotImplementedException();//_baseRepository.SaveChanges();
    }

    public void Update(LicenseType entity)
    {
        _baseRepository.Update(entity);
    }
}
