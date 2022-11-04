using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class RemoveRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class RemoveOperatorFromVehicle : ICommand
{
    internal int VehicleId { get; private set; }
    internal int OperatorId { get; private set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class RemoveVehicleFromOperator : ICommand
{
    internal int VehicleId { get; private set; }
    internal int OperatorId { get; private set; }
}