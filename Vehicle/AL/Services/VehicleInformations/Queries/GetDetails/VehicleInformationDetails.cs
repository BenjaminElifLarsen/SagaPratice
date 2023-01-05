using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.VehicleInformations.Queries.GetDetails;
public record VehicleInformationDetails : BaseReadModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<Guid> VehicleIds { get; private set; }
    public byte MaxWheelAmount { get; private set; }
    public Guid LicenseTypeId { get; private set; }

    public VehicleInformationDetails(Guid id, string name, byte maxWheelAmount, Guid licenseTypeId, IEnumerable<Guid> vehicleIds)
    {
        Id = id;
        Name = name;
        MaxWheelAmount = maxWheelAmount;
        LicenseTypeId = licenseTypeId;
        VehicleIds = vehicleIds;
    }
}
