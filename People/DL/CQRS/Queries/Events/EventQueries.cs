using BaseRepository;
using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;
using PersonDomain.DL.Events.Domain;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
using System.Linq.Expressions;

namespace PersonDomain.DL.CQRS.Queries.Events;
internal class GenderSubjectQuery
{ //will need a new base query for events
    //public override Expression<Func<IEnumerable<Event<Guid>>, GenderSubject>> Projection()
    //{ //maybe use Where()s for getting the specific events over in the base concrete repo?
    //    //need to combine the events together
    //    //maybe it could be a good idea not to consider how cqrs is done with a domain model fully persistanced in the context
    //}

    //public override Expression<Func<Event<Guid>, DomainEvent>> Projection()
    //{ //convert the events to domain events
    //    //do different things on them depending on what types they are
    //    //would need to handle every single type and have to update code each time an domain evnet is added, updated, or removed.
    //    //got GenderConversion.Set, maybe such could help
    //    //the DataPoints by themselves sadly will not work as they are enum based and the store holds events of different boundared contexes
    //    //will require to work with any ORMS (and non-ORMS), so switch cases and such are out of the picture as they might be able to be translated

    //}

    public /*override*/ Expression<Func<Event<Guid>, GenderSubject, GenderSubject>> Projection() //cannot be used Select()
    {
        return (e, t) =>
        e.Type == nameof(GenderRecognisedSucceeded) ? new(e.Data.SingleOrDefault(x => x == GenderPropertyId.VerbSubject).Value) : t; 
        //{
        //    if(e.Type == nameof(GenderRecognisedSucceeded))
        //    {
        //        return new(e.Data.SingleOrDefault(x => x == GenderPropertyId.VerbSubject).Value);
        //    }
        //    else
        //    {
        //        return t;
        //    }
        //};
    }


    //could return the events and hydrate a different class than the domain model

    //https://domaincentric.net/blog/event-sourcing-projections
    //# definition of project, last part 
}

internal class PersonGenderQuery
{
}

public interface IQueryBaseTest<T> where T : IProjection
{
    public T Projection(IEnumerable<DomainEvent> events);
}
public interface IProjection
{

}

internal class QueryBaseTest : IQueryBaseTest<GenderSubject>
{
    public GenderSubject Projection(IEnumerable<DomainEvent> events)
    {
        return GenderSubject.Projection(events);
        //GenderSubject? testing = default;
        //foreach(var e in events)
        //{
        //    switch(e.EventType)
        //    {
        //        case nameof(GenderRecognisedSucceeded):
        //            var gr = e as GenderRecognisedSucceeded;
        //            if (testing != default) throw new Exception($"Error: Duplicated {nameof(GenderRecognisedSucceeded)}.");
        //            testing = new(gr.Subject);
        //            break;
        //    }
        //}
        //return testing;
    }
}

internal static class TestExtension
{
    public static T Projection<T>(this IEnumerable<DomainEvent> events, IQueryBaseTest<T> query) where T : IProjection
    {
        return query.Projection(events);
    }
}