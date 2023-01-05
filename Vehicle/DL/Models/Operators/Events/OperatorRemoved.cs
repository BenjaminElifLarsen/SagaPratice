using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorRemoved : DomainEvent
{
    public IEnumerable<Guid> VehicleIds { get; private set; }
    
    public IEnumerable<Guid> LicenseTypeIds { get; private set; }

    public OperatorRemoved(Operator aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
        VehicleIds = aggregate.Vehicles;
        LicenseTypeIds = aggregate.Licenses.Select(x => x.Type);
    }
}