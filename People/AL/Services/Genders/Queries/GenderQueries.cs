using Common.CQRS.Queries;
using PeopleDomain.AL.Services.Genders.Queries.GetDetails;
using PeopleDomain.AL.Services.Genders.Queries.GetList;
using PeopleDomain.DL.Models;
using System.Linq.Expressions;

namespace PeopleDomain.AL.Services.Genders.Queries;
internal sealed class GenderListItemQuery : BaseQuery<Gender, GenderListItem>
{
    public override Expression<Func<Gender, GenderListItem>> Map()
    {
        return e => new(e.Id, e.VerbSubject, e.VerbObject, e.People.Count());
    }
}

internal sealed class GenderDetailsQuery : BaseQuery<Gender, GenderDetails>
{
    public override Expression<Func<Gender, GenderDetails>> Map()
    {
        return e => new(e.Id, e.VerbSubject, e.VerbObject, e.People.Select(x => x.Id));
    }
}
