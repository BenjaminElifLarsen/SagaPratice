using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.Vehicles.Queries.GetList;
public record VehicleListItem : BaseReadModel
{
    public int Id { get; private set; }
    public string SerialNumber { get; private set; }
    public int NumberOfOperators { get; private set; }

    public VehicleListItem(int id, string serialNumber, int operatorAmount)
    {
        Id = id;
        SerialNumber = serialNumber;
        NumberOfOperators = operatorAmount;
    }
}
