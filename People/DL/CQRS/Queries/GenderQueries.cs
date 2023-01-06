using Common.CQRS.Queries;
using PersonDomain.DL.CQRS.Queries.ReadModels;
using PersonDomain.DL.Models;
using System.Linq.Expressions;

namespace PersonDomain.DL.CQRS.Queries;
internal sealed class GenderIdQuery : BaseQuery<Gender, GenderIdValidation>
{
    public override Expression<Func<Gender, GenderIdValidation>> Map()
    {
        return e => new(e.Id);
    }
}

internal sealed class GenderVerbQuery : BaseQuery<Gender, GenderVerbValidation>
{
    public override Expression<Func<Gender, GenderVerbValidation>> Map()
    {
        return e => new(e.VerbObject, e.VerbSubject);
    }
}