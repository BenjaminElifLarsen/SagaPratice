using Common.Events.Domain;
using Common.RepositoryPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Services;
internal class UnitOfWork : IUnitOfWork
{
    private readonly ILicenseTypeRepository _licenseTypeRepository;
    private readonly IOperatorRepository _operatorRepository;
    private readonly IVehicleInformationRepository _vehicleInformationRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDomainEventBus _eventBus;

    public ILicenseTypeRepository LicenseTypeRepository => throw new NotImplementedException();

    public IOperatorRepository OperatorRepository => throw new NotImplementedException();

    public IVehicleInformationRepository VehicleInformationRepository => throw new NotImplementedException();

    public IVehicleRepository VehicleRepository => throw new NotImplementedException();

    public void Save()
    {
        //var roots = (_licenseTypeRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>)
        //    .Concat(_operatorRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>)
        //    .Concat(_vehicleInformationRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>)
        //    .Concat(_vehicleRepository.GetTrackedAsync().Result as IEnumerable<IAggregateRoot>)
        //    .ToArray();
        //for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
        //{
        //    for (int n = 0; n < roots[i].Events.Count(); n++)
        //    {
        //        _eventBus.Publish(roots[i].Events.ToArray()[n]);
        //        roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
        //    }
        //}
        _licenseTypeRepository.Save();
    }
}
