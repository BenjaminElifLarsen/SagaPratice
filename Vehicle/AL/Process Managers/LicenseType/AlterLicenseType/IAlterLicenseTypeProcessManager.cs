using Common.ProcessManager;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal interface IAlterLicenseTypeProcessManager : IProcessManager,
    IProcessManagerEventHandler<LicenseTypeAlteredSuccessed>,
    IProcessManagerEventHandler<LicenseTypeAlteredFailed>,
    IProcessManagerEventHandler<LicenseTypeAgeRequirementChanged>,
    IProcessManagerEventHandler<LicenseTypeRenewPeriodChanged>,
    IProcessManagerEventHandler<OperatorForAgeValidatioNotFound>,
    IProcessManagerEventHandler<OperatorForRenewValidationNotFound>,
    IProcessManagerEventHandler<OperatorLicenseValidated>,
    IProcessManagerEventHandler<OperatorLicenseRetracted>,
    IProcessManagerEventHandler<OperatorLicenseExpired>
{
}
