using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorLicenseRenewed : IDomainEvent<OperatorLicenseRenewedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorLicenseRenewedData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal OperatorLicenseRenewed()
    {

    }
}

public class OperatorLicenseRenewedData
{

}