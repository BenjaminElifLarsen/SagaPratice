using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseTypeRepository : ILicenseTypeRepository
{
    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel
    {
        throw new NotImplementedException();
    }

    public void Create(LicenseType entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(LicenseType entity)
    {
        throw new NotImplementedException();
    }

    public Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<LicenseType, TProjection> query) where TProjection : BaseReadModel
    {
        throw new NotImplementedException();
    }

    public Task<LicenseType> GetForOperationAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsIdUniqueAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Update(LicenseType entity)
    {
        throw new NotImplementedException();
    }
}
