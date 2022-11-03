using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class UseVehicleFromUser : ICommand
{
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
}
