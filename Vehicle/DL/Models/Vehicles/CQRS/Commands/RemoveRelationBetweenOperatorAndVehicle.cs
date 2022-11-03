using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class RemoveRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class RemoveOperatorFromVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class RemoveVehicleFromOperator : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}