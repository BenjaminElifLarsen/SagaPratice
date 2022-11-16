using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
internal class OperatorLicenseRenewed : IDomainEvent<OperatorLicenseRenewedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorLicenseRenewedData Data { get; private set; }

    internal OperatorLicenseRenewed()
    {

    }
}

internal class OperatorLicenseRenewedData
{

}