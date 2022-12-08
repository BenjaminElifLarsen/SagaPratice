using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
internal class VehiclesFoundWithSpecificVehicleInformationAndOperator : IDomainEvent<VehiclesFoundWithSpecificVehicleInformationAndOperatorData>
{
    public VehiclesFoundWithSpecificVehicleInformationAndOperatorData Data => throw new NotImplementedException();

    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();
}

internal class VehiclesFoundWithSpecificVehicleInformationAndOperatorData
{
    public int OperatorId { get; private set; }
    public IEnumerable<int> VehicleIds { get; private set; }

    public VehiclesFoundWithSpecificVehicleInformationAndOperatorData(int operatorId, IEnumerable<int> vehicleIds)
    {
        OperatorId = operatorId;
        VehicleIds = vehicleIds;
    }
}