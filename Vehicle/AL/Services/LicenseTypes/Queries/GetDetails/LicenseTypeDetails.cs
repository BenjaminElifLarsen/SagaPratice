using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.LicenseTypes.Queries.GetDetails;
public record LicenseTypeDetails : BaseReadModel
{
    public int Id { get; private set; }
    public string Type { get; private set; }
    public byte RenewPeriod { get; private set; }
    public byte AgeRequirement { get; private set; }
    public DateTime CanBeIssuedFrom { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }
    public IEnumerable<int> VehicleInformationIds { get; private set; }

    public LicenseTypeDetails(int id, string type, byte renewPeriod, byte ageRequirement, DateTime canBeIssuedFrom, IEnumerable<int> operatorIds, IEnumerable<int> vehicleInformationIds)
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
