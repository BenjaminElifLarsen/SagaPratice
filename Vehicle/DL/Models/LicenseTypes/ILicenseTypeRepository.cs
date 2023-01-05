using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.LicenseTypes;
public interface ILicenseTypeRepository
{
    Task<bool> IsIdUniqueAsync(Guid id);
    Task<bool> IsTypeUniqueAsync(string type);
    Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<LicenseType,TProjection> query) where TProjection: BaseReadModel;
    void Create(LicenseType entity);
    void Update(LicenseType entity);
    void Delete(LicenseType entity);
    Task<LicenseType> GetForOperationAsync(Guid id);
}
