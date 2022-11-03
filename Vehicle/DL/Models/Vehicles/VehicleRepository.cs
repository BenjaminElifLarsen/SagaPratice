using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Vehicles;
internal class VehicleRepository : IVehicleRepository
{
    private readonly IBaseRepository<Vehicle> _baseRepository;

    public VehicleRepository(IBaseRepository<Vehicle> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public void Create(Vehicle entity)
    {
        _baseRepository.Create(entity);
    }

    public async Task<IEnumerable<TProjection>> CurrentlyOperating<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllByPredicateAsync(x => x.InUse == true, query);
    }

    public void Delete(Vehicle entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<bool> DoesVehicleExist(int id)
    {
        return !await _baseRepository.IsUniqueAsync(x => x.VehicleId == id);
    }

    public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.VehicleId == id, query);
    }

    public async Task<Vehicle> GetForOperationAsync(int id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.VehicleId == id);
    }

    public void Save()
    {
        _baseRepository.SaveChanges();
    }

    public void Update(Vehicle entity)
    {
        _baseRepository.Update(entity);
    }
}
