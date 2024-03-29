﻿using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.VehicleInformations;
internal class VehicleInformationRepository : IVehicleInformationRepository
{
    private readonly IBaseRepository<VehicleInformation> _baseRepository;
    public VehicleInformationRepository(IBaseRepository<VehicleInformation> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public void Create(VehicleInformation entity)
    {
        _baseRepository.Create(entity);
    }

    public void Delete(VehicleInformation entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<IEnumerable<TProjection>> FindAllWithSpecificLicenseTypeId<TProjection>(Guid licenseTypeId, BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllByPredicateAsync(x => x.LicenseTypeRequired == licenseTypeId, query);
    }

    public async Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<VehicleInformation, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.Id == id, query);
    }

    public async Task<VehicleInformation> GetForOperationAsync(Guid id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.Id == id);
    }

    public Task<bool> IsNameUniqueAsync(string name)
    {
        return _baseRepository.IsUniqueAsync(x => x.Name == name);
    }

    public void Update(VehicleInformation entity)
    {
        _baseRepository.Update(entity);
    }
}
