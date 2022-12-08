using Common.ProcessManager;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
public interface IAlterLicenseTypeProcessManager : IProcessManager,
    IProcessManagerEventHandler<LicenseTypeAlteredSuccessed>,
    IProcessManagerEventHandler<LicenseTypeAlteredFailed>,
    IProcessManagerEventHandler<LicenseTypeAgeRequirementChanged>,
    IProcessManagerEventHandler<LicenseTypeRenewPeriodChanged>,
    IProcessManagerEventHandler<LicenseTypeOperatorRemoved>,
    IProcessManagerEventHandler<OperatorForAgeValidatioNotFound>,
    IProcessManagerEventHandler<OperatorForRenewValidationNotFound>,
    IProcessManagerEventHandler<OperatorLicenseAgeRequirementValidated>,
    IProcessManagerEventHandler<OperatorLicenseRetracted>,
    IProcessManagerEventHandler<OperatorLicenseExpired>,
    IProcessManagerEventHandler<OperatorLicenseRenewPeriodValidated>,
    IProcessManagerEventHandler<OperatorRemovedVehicle>,
    IProcessManagerEventHandler<VehicleRemovedOperator>,
    IProcessManagerEventHandler<VehicleNotRequiredToRemoveOperator>,
    IProcessManagerEventHandler<VehiclesFoundWithSpecificVehicleInformationAndOperator>,
    IProcessManagerEventHandler<FoundVehicleInformations>
{
}
