using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.VehicleInformations.Queries.GetList;
public record VehicleInformationListItem : BaseReadModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int NumberOfRegistractedVehicles { get; private set; }
    public VehicleInformationListItem(Guid id, string name, int numberOfRegistractedVehicles)
    {
        Id = id;
        Name = name;
        NumberOfRegistractedVehicles = numberOfRegistractedVehicles;
    }
}
