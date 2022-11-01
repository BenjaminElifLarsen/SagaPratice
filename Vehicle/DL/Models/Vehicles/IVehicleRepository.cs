﻿using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles;
internal interface IVehicleRepository
{
    Task<bool> DoesVehicleExist(int id);
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Vehicle, TProjection> query) where TProjection : BaseReadModel;
    void Save();
    void Create(Vehicle entity);
    void Update(Vehicle entity);
    void Delete(Vehicle entity);
    Task<Vehicle> GetForOperationAsync(int id);
}