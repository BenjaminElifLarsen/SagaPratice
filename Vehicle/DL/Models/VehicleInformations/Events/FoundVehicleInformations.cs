using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public sealed record FoundVehicleInformations : SystemEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public int OperatorId { get; private set; }
    
    public IEnumerable<int> VehicleInformationIds { get; private set; }

    internal FoundVehicleInformations(int operatorId, IEnumerable<int> vehicleInformationIds, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(VehicleInformation).Name;
        AggregateId = 0;
        OperatorId = operatorId;
        VehicleInformationIds = vehicleInformationIds;
    }
}