using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
public class OperatorDetails : BaseReadModel
{
    public int Id { get; private set; }
    public DateOnly Birthday { get; private set; }
    public IEnumerable<int> VehicleIds { get; private set; }
    public IEnumerable<OperatorLicenseDetails> Licenses { get; private set; }

    public OperatorDetails(int id, DateOnly birthday, IEnumerable<int> vehicleIds, IEnumerable<OperatorLicenseDetails> licenses)
    {
        Id = id;
        Birthday = birthday;
        VehicleIds = vehicleIds;
        Licenses = licenses;
    }
}

public class OperatorLicenseDetails : BaseReadModel
{
    public DateOnly Arquired { get; private set; }
    public DateOnly? LastRenewed { get; private set; }
    public bool Expired { get; private set; }
    public int TypeId { get; private set; }

    public OperatorLicenseDetails(DateOnly arquired, DateOnly? lastRenewed, bool expired, int type)
    {
        Arquired = arquired;
        LastRenewed = lastRenewed;
        Expired = expired;
        TypeId = type;
    }
}


