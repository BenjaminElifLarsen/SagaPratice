using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorRemoved : DomainEvent
{
    public IEnumerable<int> VehicleIds { get; private set; }
    
    public IEnumerable<int> LicenseTypeIds { get; private set; }

    public OperatorRemoved(Operator aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
        VehicleIds = aggregate.Vehicles.Select(x => x.Id).ToList();
        LicenseTypeIds = aggregate.Licenses.Select(x => x.Type.Id).ToList();
    }
}