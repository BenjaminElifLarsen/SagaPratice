using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.VehicleInformations.Queries.GetDetails;
public class VehicleInformationDetails : BaseReadModel
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<int> VehicleIds { get; private set; }
    public byte MaxWheelAmount { get; private set; }
    public int LicenseTypeId { get; private set; }

    public VehicleInformationDetails(int id, string name, byte maxWheelAmount, int licenseTypeId, IEnumerable<int> vehicleIds)
    {
        Id = id;
        Name = name;
        MaxWheelAmount = maxWheelAmount;
        LicenseTypeId = licenseTypeId;
        VehicleIds = vehicleIds;
    }
}
