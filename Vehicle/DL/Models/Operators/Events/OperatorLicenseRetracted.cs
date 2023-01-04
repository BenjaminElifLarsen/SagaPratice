using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseRetracted : DomainEvent
{
    public int LicenseTypeId { get; private set; } //license type id of the retracted license
    public IEnumerable<int> VehicleIds { get; private set; }
    internal OperatorLicenseRetracted(Operator aggregate, License license, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = license.Type.Id;
        VehicleIds = aggregate.Vehicles.Select(x => x.Id);
    }
}