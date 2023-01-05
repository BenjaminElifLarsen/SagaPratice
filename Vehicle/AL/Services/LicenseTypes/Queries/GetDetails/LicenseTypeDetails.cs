using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.LicenseTypes.Queries.GetDetails;
public record LicenseTypeDetails : BaseReadModel
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public byte RenewPeriod { get; private set; }
    public byte AgeRequirement { get; private set; }
    public DateTime CanBeIssuedFrom { get; private set; }
    public IEnumerable<Guid> OperatorIds { get; private set; }
    public IEnumerable<Guid> VehicleInformationIds { get; private set; }

    public LicenseTypeDetails(Guid id, string type, byte renewPeriod, byte ageRequirement, DateTime canBeIssuedFrom, IEnumerable<Guid> operatorIds, IEnumerable<Guid> vehicleInformationIds)
    {
        Id = id;
        Type = type;
        RenewPeriod = renewPeriod;
        AgeRequirement = ageRequirement;
        CanBeIssuedFrom = canBeIssuedFrom;
        OperatorIds = operatorIds;
        VehicleInformationIds = vehicleInformationIds;
    }
}
