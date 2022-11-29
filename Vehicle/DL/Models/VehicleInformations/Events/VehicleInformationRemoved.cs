using Common.Events.Domain;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public class VehicleInformationRemoved : IDomainEvent //the code that triggers this should not be able to delete the entity as long time a vehicle reference it
{
    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();
}
