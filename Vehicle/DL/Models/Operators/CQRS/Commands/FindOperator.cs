﻿using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
internal class FindOperator : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int OperatorId { get; private set; }

    public FindOperator(int operatorId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}