using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.AL.Services.Genders.Queries.GetDetailsOverTime;

namespace PersonDomain.AL.Services.Genders.Queries;

internal class GenderOverTimeView : IViewSingleQuery<GenderOverTime>, IViewMultiQuery<GenderOverTime>
{
    public GenderOverTime SingleProjection(IEnumerable<DomainEvent> events)
    {
        return GenderOverTime.SingleProjection(events);
    }

    public IEnumerable<GenderOverTime> MultiProjection(IEnumerable<DomainEvent> events)
    {
        return GenderOverTime.MultiProjection(events);
    }
}