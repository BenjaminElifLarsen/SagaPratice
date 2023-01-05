using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class FindVehicle : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public Guid VehicleId { get; private set; }

    internal FindVehicle(Guid vehicleId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
