using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
[Obsolete("Might be removed in a future version", true)]
internal sealed record LicenseTypeRenewPeriodValidated : DomainEvent
{ //not needed if not validating multiple aggregates via a single command
    public IEnumerable<int> OperatorIdsNotFound { get; private set; }
    public IEnumerable<int> OperatorIdsNotValid { get; private set; }

    public LicenseTypeRenewPeriodValidated(LicenseType aggregate, IEnumerable<int> operatorIdsNotFound, IEnumerable<int> operatorIdsNotValid, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorIdsNotFound = operatorIdsNotFound;
        OperatorIdsNotValid = operatorIdsNotValid;
    }
}