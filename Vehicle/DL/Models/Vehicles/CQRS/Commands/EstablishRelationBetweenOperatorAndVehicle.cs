﻿using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class EstablishRelationBetweenOperatorAndVehicle : ICommand
{
    public Guid VehicleId { get; set; }
    public Guid OperatorId { get; set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public EstablishRelationBetweenOperatorAndVehicle()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class AddOperatorToVehicle : ICommand
{ //need in all cases to validate that the operator got the needed license for the vehicle
    internal Guid VehicleId { get; private set; }
    internal Guid OperatorId { get; private set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }


    public AddOperatorToVehicle(Guid vehicleId, Guid operatorId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        OperatorId = operatorId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}

/// <summary>
/// For internal use only, should be created by a triggered event.
/// </summary>
public class AddVehicleToOperator : ICommand
{
    internal Guid VehicleId { get; private set; }
    internal Guid OperatorId { get; private set; }
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AddVehicleToOperator(Guid vehicleId, Guid operatorId, Guid correlationId, Guid causationId)
    {
        VehicleId = vehicleId;
        OperatorId = operatorId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
