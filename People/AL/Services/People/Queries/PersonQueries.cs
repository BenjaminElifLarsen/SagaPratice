using Common.CQRS.Queries;
using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.AL.Services.People.Queries.GetDetails;
using PersonDomain.AL.Services.People.Queries.GetList;
using PersonDomain.AL.Services.People.Queries.GetPeoplesGendersOverTime;
using PersonDomain.DL.Models;
using System.Linq.Expressions;

namespace PersonDomain.AL.Services.People.Queries;
internal sealed class PersonListItemQuery : BaseQuery<Person, PersonListItem>
{
    public override Expression<Func<Person, PersonListItem>> Map()
    {
        return e => new(e.Id, e.FirstName + " " + e.LastName, e.Birth, e.Gender);
    }
}

internal sealed class PersonDetailsQuery : BaseQuery<Person, PersonDetails>
{
    public override Expression<Func<Person, PersonDetails>> Map()
    {
        return e => new(e.Id, e.FirstName, e.LastName, e.Birth, e.Gender);
    }
}

internal class PersonGenderChangesView : IViewSingleQuery<PersonGenderChanges>, IViewMultiQuery<PersonGenderChanges>
{
    public IEnumerable<PersonGenderChanges> MultiProjection(IEnumerable<DomainEvent> events)
    {
        return PersonGenderChanges.MultiProjection(events);
    }

    public PersonGenderChanges SingleProjection(IEnumerable<DomainEvent> events)
    {
        return PersonGenderChanges.SingleProjection(events);
    }
}