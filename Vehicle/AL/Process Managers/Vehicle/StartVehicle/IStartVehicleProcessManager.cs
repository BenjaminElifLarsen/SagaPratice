using Common.ProcessManager;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.Vehicle.StartVehicle;
public interface IStartVehicleProcessManager : IProcessManager,
    IProcessManagerEventHandler<OperatorNotFound>,
    IProcessManagerEventHandler<VehicleNotFound>,
    IProcessManagerEventHandler<OperatorWasFound>,
    IProcessManagerEventHandler<VehicleWasFound>,
    IProcessManagerEventHandler<VehicleStartedSucceeded>,
    IProcessManagerEventHandler<VehicleStartedFailed>,
    IProcessManagerEventHandler<NotPermittedToOperate>,
    IProcessManagerEventHandler<PermittedToOperate>,
    IProcessManagerEventHandler<OperatorLackedNeededLicense>,
    IProcessManagerEventHandler<OperatorLicenseExpired>,
    IProcessManagerEventHandler<AttemptToStartVehicleStarted>,
    IProcessManagerEventHandler<VehicleNotRequiredToRemoveOperator>,
    IProcessManagerEventHandler<VehicleRemovedOperator>
{
}
