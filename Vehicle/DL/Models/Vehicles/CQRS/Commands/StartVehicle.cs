using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
internal class StartVehicle : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }
    
    public int VehicleId { get; private set; }

    public StartVehicle(int vehicleId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
