using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class AttemptToStartVehicle : ICommand
{
    public Guid VehicleId { get; set; }
    public Guid OperatorId { get; set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AttemptToStartVehicle()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

public class StopOperatingVehicle : ICommand
{
    public Guid VehicleId { get; set; }
    public Guid OperatorId { get; set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public StopOperatingVehicle()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
