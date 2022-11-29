using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class EstablishRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public EstablishRelationBetweenOperatorAndVehicle()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class AddOperatorToVehicle : ICommand
{ //need in all cases to validate that the operator got the needed license for the vehicle
    internal int VehicleId { get; private set; }
    internal int OperatorId { get; private set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }


    public AddOperatorToVehicle(int vehicleId, int operatorId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        OperatorId = operatorId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class AddVehicleToOperator : ICommand
{
    internal int VehicleId { get; private set; }
    internal int OperatorId { get; private set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AddVehicleToOperator(int vehicleId, int operatorId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        OperatorId = operatorId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
