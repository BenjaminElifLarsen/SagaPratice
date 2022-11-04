﻿using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class StartOperatingVehicle : ICommand
{
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
}

public class StopOperatingVehicle : ICommand
{
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
}