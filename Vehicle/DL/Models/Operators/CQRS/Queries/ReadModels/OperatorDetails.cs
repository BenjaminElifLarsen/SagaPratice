using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
public record OperatorDetails : BaseReadModel
{
    public Guid Id { get; private set; }
    public DateOnly Birthday { get; private set; }
    public IEnumerable<Guid> VehicleIds { get; private set; }
    public IEnumerable<OperatorLicenseDetails> Licenses { get; private set; }

    public OperatorDetails(Guid id, DateOnly birthday, IEnumerable<Guid> vehicleIds, IEnumerable<OperatorLicenseDetails> licenses)
    {
        Id = id;
        Birthday = birthday;
        VehicleIds = vehicleIds;
        Licenses = licenses;
    }
}

public record OperatorLicenseDetails : BaseReadModel
{
    public DateOnly Arquired { get; private set; }
    public DateOnly? LastRenewed { get; private set; }
    public bool Expired { get; private set; }
    public Guid TypeId { get; private set; }

    public OperatorLicenseDetails(DateOnly arquired, DateOnly? lastRenewed, bool expired, Guid type)
    {
        Arquired = arquired;
        LastRenewed = lastRenewed;
        Expired = expired;
        TypeId = type;
    }
}


