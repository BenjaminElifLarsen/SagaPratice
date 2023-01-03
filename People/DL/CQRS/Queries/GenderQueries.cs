﻿using Common.CQRS.Queries;
using PeopleDomain.DL.CQRS.Queries.ReadModels;
using PeopleDomain.DL.Models;
using System.Linq.Expressions;

namespace PeopleDomain.DL.CQRS.Queries;
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