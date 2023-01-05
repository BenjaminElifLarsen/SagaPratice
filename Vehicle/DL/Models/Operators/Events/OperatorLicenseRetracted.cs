using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseRetracted : DomainEvent
{
    public Guid LicenseTypeId { get; private set; } //license type id of the retracted license
    public IEnumerable<Guid> VehicleIds { get; private set; }
    internal OperatorLicenseRetracted(Operator aggregate, License license, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = license.Type;
        VehicleIds = aggregate.Vehicles;
    }
}