using Common.Events.Bus;
using Common.ProcessManager;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Process_Managers.Vehicle.StartVehicle;
public interface IStartVehicleProcessManager : IProcessManager,
    IEventHandler<OperatorNotFound>,
    IEventHandler<VehicleNotFound>,
    IEventHandler<OperatorWasFound>,
    IEventHandler<VehicleWasFound>,
    IEventHandler<VehicleStartedSucceeded>,
    IEventHandler<VehicleStartedFailed>,
    IEventHandler<NotPermittedToOperate>,
    IEventHandler<PermittedToOperate>,
    IEventHandler<OperatorLackedNeededLicense>,
    IEventHandler<OperatorLicenseExpired>,
    IEventHandler<AttemptToStartVehicleStarted>,
    IEventHandler<VehicleNotRequiredToRemoveOperator>,
    IEventHandler<VehicleRemovedOperator>
{
}
