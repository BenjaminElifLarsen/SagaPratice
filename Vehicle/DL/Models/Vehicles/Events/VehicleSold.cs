using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
internal class VehicleSold : IDomainEvent<VehicleSoldData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public VehicleSoldData Data { get; private set; }

    internal VehicleSold(Vehicle aggregate)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.VehicleId);
    }
}

internal class VehicleSoldData
{
    public int Id { get; private set; }

	public VehicleSoldData(int id)
	{
		Id = id;
	}
}