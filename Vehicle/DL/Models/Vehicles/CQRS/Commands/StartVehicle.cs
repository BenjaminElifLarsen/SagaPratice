using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class StartVehicle : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }
    
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }

    internal StartVehicle(int vehicleId, int operatorId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
        OperatorId = operatorId;
    }
}
