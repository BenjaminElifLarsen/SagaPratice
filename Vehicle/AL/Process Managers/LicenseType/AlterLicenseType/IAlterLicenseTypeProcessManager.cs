using Common.ProcessManager;
using VehicleDomain.DL.Models.LicenseTypes.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal interface IAlterLicenseTypeProcessManager : IProcessManager,
    IProcessManagerEventHandler<LicenseTypeAltered>//,
    //might be best to 'start from scrats' regarding events
    //figure out what is needed and the same for commands.
    //data that can be altered, renew period and age requirement
    //if age requirement is changed, need to validate all licenses.
    //if a license is invalid because of age it should be removed.
    //thus the license type need to know which operators to remove.
    //transmit back a list of license operators that should be removed (caused by age requirement change).

{
}
