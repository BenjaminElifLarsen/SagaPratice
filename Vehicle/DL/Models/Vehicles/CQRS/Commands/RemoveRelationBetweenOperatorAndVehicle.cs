using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class RemoveRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

internal class RemoveOperatorFromVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

internal class RemoveVehicleFromOperator : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}