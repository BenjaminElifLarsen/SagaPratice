using BaseRepository;
using Common.RepositoryPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
public interface IVehicleContext : IBaseContext, 
    IContextData<Operator>,
    IContextData<LicenseType>,
    IContextData<VehicleInformation>,
    IContextData<Vehicle>
{
}
