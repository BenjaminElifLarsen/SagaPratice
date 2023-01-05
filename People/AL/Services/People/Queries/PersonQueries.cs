using Common.CQRS.Queries;
using PeopleDomain.AL.Services.People.Queries.GetDetails;
using PeopleDomain.AL.Services.People.Queries.GetList;
using PeopleDomain.DL.Models;
using System.Linq.Expressions;

namespace PeopleDomain.AL.Services.People.Queries;
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
