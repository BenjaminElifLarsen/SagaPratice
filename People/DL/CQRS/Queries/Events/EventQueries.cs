using Common.Events.Domain;
using Common.Events.Projection;
using Common.Events.Store.Event;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;

namespace PersonDomain.DL.CQRS.Queries.Events;

internal class PersonGenderChangesView : IViewQuery<PersonGenderChanges>
{
    public PersonGenderChanges Projection(IEnumerable<DomainEvent> events)
    {
        return PersonGenderChanges.Projection(events); 
    }
}

internal class GenderSubjectView : IViewQuery<GenderSubject>
{
    public GenderSubject Projection(IEnumerable<DomainEvent> events)
    {
        return GenderSubject.Projection(events);
    }
}