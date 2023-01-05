using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
[Obsolete("Might be removed in a future version", true)]
internal sealed record LicenseTypeAgeRequirementValidated : DomainEvent
{ //not needed if not validating multiple aggregates via a single command
    public IEnumerable<Guid> OperatorIdsNotFound { get; private set; }
    public IEnumerable<Guid> OperatorIdsNotValid { get; private set; }

    internal LicenseTypeAgeRequirementValidated(LicenseType aggregate, IEnumerable<Guid> operatorIdsNotFound, IEnumerable<Guid> operatorIdsNotValid, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorIdsNotFound = operatorIdsNotFound;
        OperatorIdsNotValid = operatorIdsNotValid;
    }
}