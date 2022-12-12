using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
internal class CheckIfVehicleIsAlreadyStarted : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int VehicleId { get; private set; }

    public CheckIfVehicleIsAlreadyStarted(int vehicleId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
