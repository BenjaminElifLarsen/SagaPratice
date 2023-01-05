using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record NotPermittedToOperate : SystemEvent
{
    public Guid Id { get; private set; }
    internal NotPermittedToOperate(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        Id = aggregate.Id;
    }
}
