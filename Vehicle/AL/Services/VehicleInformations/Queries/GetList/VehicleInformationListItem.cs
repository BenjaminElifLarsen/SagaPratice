using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.VehicleInformations.Queries.GetList;
public record VehicleInformationListItem : BaseReadModel
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int NumberOfRegistractedVehicles { get; private set; }
    public VehicleInformationListItem(int id, string name, int numberOfRegistractedVehicles)
    {
        Id = id;
        Name = name;
        NumberOfRegistractedVehicles = numberOfRegistractedVehicles;
    }
}
