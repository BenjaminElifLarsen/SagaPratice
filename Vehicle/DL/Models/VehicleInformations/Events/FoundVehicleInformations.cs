using Common.Events.Domain;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
internal class FoundVehicleInformations : IDomainEvent<FoundVehicleInformationsData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public FoundVehicleInformationsData Data { get; private set; }

    public FoundVehicleInformations(IEnumerable<int> vehicleInformationIds, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(VehicleInformation).Name;
        AggregateId = 0;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Data = new(vehicleInformationIds);
    }
}

public class FoundVehicleInformationsData
{
    public IEnumerable<int> VehicleInformationIds { get; private set; }

    public FoundVehicleInformationsData(IEnumerable<int> vehicleInformationIds)
    {
        VehicleInformationIds = vehicleInformationIds;
    }
}