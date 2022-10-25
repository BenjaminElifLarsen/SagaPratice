using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.VehicleInformations;
internal class VehicleInformationRepository : IVehicleInformationRepository
{
    private readonly IBaseRepository<VehicleInformation> _baseRepository;
    public VehicleInformationRepository(IBaseRepository<VehicleInformation> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel
    {
        return _baseRepository.AllAsync(query);
    }

    public void Create(VehicleInformation entity)
    {
        _baseRepository.Create(entity);
    }

    public void Delete(VehicleInformation entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<VehicleInformation> GetForOperationAsync(int id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.VehicleInformationId == id);
    }

    public Task<bool> IsNameUniqueAsync(string name)
    {
        return _baseRepository.IsUniqueAsync(x => x.Name == name);
    }

    public void Save()
    {
        _baseRepository.SaveChanges();
    }

    public void Update(VehicleInformation entity)
    {
        _baseRepository.Update(entity);
    }
}
