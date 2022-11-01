using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class RemoveRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

public class RemoveOperatorFromVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

public class RemoveVehicleFromOperator : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}