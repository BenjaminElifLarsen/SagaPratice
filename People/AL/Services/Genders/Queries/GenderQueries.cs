using Common.CQRS.Queries;
using PersonDomain.AL.Services.Genders.Queries.GetDetails;
using PersonDomain.AL.Services.Genders.Queries.GetList;
using PersonDomain.DL.Models;
using System.Linq.Expressions;

namespace PersonDomain.AL.Services.Genders.Queries;
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
        return e => new(e.Id, e.VerbSubject, e.VerbObject, e.People.Select(x => x));
    }
}
