using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class EstablishRelationBetweenOperatorAndVehicle : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
internal class AddOperatorToVehicle : ICommand
{ //need in all cases to validate that the operator got the needed license for the vehicle
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
internal class AddVehicleToOperator : ICommand
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
}
