using Common.Events.Domain;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Handlers.Event;
public interface IVehicleEventHandler :
    IDomainEventHandler<LicenseTypeAgeRequirementChanged>,
    IDomainEventHandler<LicenseTypeRenewPeriodChanged>,
    IDomainEventHandler<LicenseTypeRetracted>,
    IDomainEventHandler<OperatorAdded>,
    IDomainEventHandler<OperatorGainedLicense>,
    IDomainEventHandler<OperatorLicenseExpired>,
    IDomainEventHandler<OperatorLicenseRenewed>,
    IDomainEventHandler<OperatorLicenseRetracted>,
    IDomainEventHandler<OperatorRemoved>,
    IDomainEventHandler<VehicleInformationAdded>,
    IDomainEventHandler<VehicleInformationRemoved>,
    IDomainEventHandler<VehicleBought>,
    IDomainEventHandler<VehicleOperatorRelationshipEstablished>,
    IDomainEventHandler<VehicleOperatorRelationshipDisbanded>,
    IDomainEventHandler<VehicleSold>
{
}
