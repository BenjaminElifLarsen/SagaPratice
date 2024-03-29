﻿using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.VehicleInformations;
public interface IVehicleInformationRepository
{
    Task<bool> IsNameUniqueAsync(string name);
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<VehicleInformation,TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> FindAllWithSpecificLicenseTypeId<TProjection>(Guid licenseTypeId, BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel;
    void Create(VehicleInformation entity);
    void Update(VehicleInformation entity);
    void Delete(VehicleInformation entity);
    Task<VehicleInformation> GetForOperationAsync(Guid id);
    Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel;
}
