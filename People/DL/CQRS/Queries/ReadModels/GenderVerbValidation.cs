﻿using Common.CQRS.Queries;

namespace PeopleDomain.DL.CQRS.Queries.ReadModels;
internal class GenderVerbValidation : BaseReadModel
{
    public string Object { get; private set; }
    public string Subject { get; private set; }
    public GenderVerbValidation(string @object, string subject)
    {
        Object = @object;
        Subject = subject;
    }
}