using Common.Events.Bus;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Handlers.Event;
public interface IVehicleEventHandler :
    IEventHandler<LicenseTypeAgeRequirementChanged>,
    IEventHandler<LicenseTypeRenewPeriodChanged>,
    IEventHandler<LicenseTypeRetracted>,
    IEventHandler<OperatorAdded>,
    IEventHandler<OperatorGainedLicense>,
    IEventHandler<OperatorLicenseExpired>,
    IEventHandler<OperatorLicenseRenewed>,
    IEventHandler<OperatorLicenseRetracted>,
    IEventHandler<OperatorRemoved>,
    IEventHandler<VehicleInformationAdded>,
    IEventHandler<VehicleInformationRemoved>,
    IEventHandler<VehicleBought>,
    IEventHandler<VehicleOperatorRelationshipEstablished>,
    IEventHandler<VehicleOperatorRelationshipDisbanded>,
    IEventHandler<VehicleSold>
{
}
