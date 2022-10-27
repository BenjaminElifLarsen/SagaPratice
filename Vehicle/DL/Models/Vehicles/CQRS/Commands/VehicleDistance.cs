using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class AddDistanceToVehicleDistance : ICommand
{
    public int Id { get; private set; }
    public double DistanceToAdd { get; private set; }
}

public class ResetVehicleMovedDistance : ICommand
{
    public int Id { get; private set; }
    public double NewDistance { get; private set; }
}
