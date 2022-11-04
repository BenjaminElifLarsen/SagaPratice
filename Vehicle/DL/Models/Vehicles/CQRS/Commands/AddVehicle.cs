using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class BuyVehicleWithOperators : ICommand
{
    public int VehicleInformation { get; set; }
    public IEnumerable<int> Operators { get; set; }
    public DateTime Produced { get; set; }
    public string SerialNumber { get; set; }
}

public class BuyVehicleWithNoOperator : ICommand
{
    public int VehicleInformation { get; set; }
    public DateTime Produced { get; set; }
    public string SerialNumber { get; set; }
}
