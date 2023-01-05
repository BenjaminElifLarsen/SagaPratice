using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class BuyVehicleWithOperators : ICommand
{
    public Guid VehicleInformation { get; set; }
    public IEnumerable<Guid> Operators { get; set; }
    public DateTime Produced { get; set; }
    public string SerialNumber { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public BuyVehicleWithOperators()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

public class BuyVehicleWithNoOperator : ICommand
{
    public Guid VehicleInformation { get; set; }
    public DateTime Produced { get; set; }
    public string SerialNumber { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public BuyVehicleWithNoOperator()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
