﻿using Common.CQRS.Commands;

namespace PersonDomain.DL.CQRS.Commands;
public sealed class UnrecogniseGender : ICommand
{
    public Guid Id { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public UnrecogniseGender()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
