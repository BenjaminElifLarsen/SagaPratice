using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;

namespace PersonDomain.DL.CQRS.Queries.Events;

internal class GenderSubjectView : IViewSingleQuery<GenderSubject>, IViewMultiQuery<GenderSubject>
{
    public GenderSubject SingleProjection(IEnumerable<DomainEvent> events)
    {
        return GenderSubject.SingleProjection(events);
    }

    public IEnumerable<GenderSubject> MultiProjection(IEnumerable<DomainEvent> events)
    {
        return GenderSubject.MultiProjection(events);
    }
}