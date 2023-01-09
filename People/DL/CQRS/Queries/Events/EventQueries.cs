using Common.Events.Store.Event;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;
using System.Linq.Expressions;

namespace PersonDomain.DL.CQRS.Queries.Events;
internal class GenderSubjectQuery
{ //will need a new base query for events
    public override Expression<Func<IEnumerable<Event<Guid>>, GenderSubject>> Projection()
    { //maybe use Where()s for getting the specific events over in the base concrete repo?
        //need to combine the events together
        //maybe it could be a good idea not to consider how cqrs is done with a domain model fully persistanced in the context
    }

}

internal class PersonGenderQuery
{

}