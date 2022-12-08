using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles;
public interface IVehicleRepository
{
    Task<bool> DoesVehicleExist(int id);
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> CurrentlyOperatingAsync<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> FindSpecificByOperatorIdAndVehicleInformationsAsync<TProjection>(int operatorId, IEnumerable<int> vehicleInformation, BaseQuery<Vehicle,TProjection> query) where TProjection : BaseReadModel;
    void Create(Vehicle entity);
    void Update(Vehicle entity);
    void Delete(Vehicle entity);
    Task<Vehicle> GetForOperationAsync(int id);
}
