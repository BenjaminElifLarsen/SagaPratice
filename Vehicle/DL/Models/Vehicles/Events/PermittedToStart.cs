using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record PermittedToOperate : SystemEvent
{
    internal PermittedToOperate(Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {

    }
}