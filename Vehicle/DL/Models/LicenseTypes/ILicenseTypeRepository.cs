using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal interface ILicenseTypeRepository
{
    Task<bool> IsIdUniqueAsync(int id);
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<LicenseType,TProjection> query) where TProjection: BaseReadModel;
    void Save();
    void Create(LicenseType entity);
    void Update(LicenseType entity);
    void Delete(LicenseType entity);
    Task<LicenseType> GetForOperationAsync(int id);
}
