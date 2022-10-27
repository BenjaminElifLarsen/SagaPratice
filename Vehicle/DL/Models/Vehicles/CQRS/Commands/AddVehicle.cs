using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class AddVehicleWithOperators : ICommand
{
    public int VehicleInformation { get; private set; }
    public IEnumerable<int> Operators { get; private set; }
    public DateTime Produced { get; private set; }
}

public class AddVehicleWithNoOperator : ICommand
{
    public int VehicleInformation { get; private set; }
    public DateTime Produced { get; private set; }
}
