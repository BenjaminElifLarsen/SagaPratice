using Common.CQRS.Queries;

namespace VehicleDomain.AL.CQRS.Queries.ReadModels;
public class VehicleInformationListItem : BaseReadModel
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public VehicleInformationListItem(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
