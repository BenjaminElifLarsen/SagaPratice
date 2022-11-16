using Common.RepositoryPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork
{
    public ILicenseTypeRepository LicenseTypeRepository { get; }
    public IOperatorRepository OperatorRepository { get; }
    public IVehicleInformationRepository VehicleInformationRepository { get; }
    public IVehicleRepository VehicleRepository { get; }
}
