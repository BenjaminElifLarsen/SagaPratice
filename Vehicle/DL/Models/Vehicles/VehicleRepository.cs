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

    public async Task<IEnumerable<TProjection>> CurrentlyOperatingAsync<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllByPredicateAsync(x => x.InUse == true, query);
    }

    public void Delete(Vehicle entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<bool> DoesVehicleExist(Guid id)
    {
        return !await _baseRepository.IsUniqueAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<TProjection>> FindSpecificByOperatorIdAndVehicleInformationsAsync<TProjection>(Guid operatorId, IEnumerable<Guid> vehicleInformations, BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllByPredicateAsync(x => x.Operators.Any(xx => xx == operatorId) && vehicleInformations.Any(xx => x.VehicleInformation == xx), query);
    }

    public async Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.Id == id, query);
    }

    public async Task<Vehicle> GetForOperationAsync(Guid id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.Id == id);
    }

    public void Update(Vehicle entity)
    {
        _baseRepository.Update(entity);
    }
}
