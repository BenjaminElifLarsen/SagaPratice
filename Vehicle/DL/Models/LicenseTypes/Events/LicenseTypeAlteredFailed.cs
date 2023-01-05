using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeAlteredFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public Guid AggregateId { get; private set; }

    public LicenseTypeAlteredFailed(LicenseType aggregate, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.Id;
        Errors = errors;
    }

    public LicenseTypeAlteredFailed(Guid id, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(LicenseType).Name;
        AggregateId = id;
        Errors = errors;
    }
}
