using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class AddDistanceToVehicleDistance : ICommand
{
    public int Id { get; set; }
    public double DistanceToAdd { get; set; }
    public Guid CommandId { get; set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AddDistanceToVehicleDistance()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

public class ResetVehicleMovedDistance : ICommand
{
    public int Id { get; set; }
    public double NewDistance { get; set; }
    public Guid CommandId { get; set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ResetVehicleMovedDistance()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
