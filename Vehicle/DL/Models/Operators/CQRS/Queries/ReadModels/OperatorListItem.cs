﻿using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
public record OperatorListItem : BaseReadModel
{
    public Guid Id { get; private set; }
    public DateOnly Birth { get; private set; }
    public int AmountOfLicense { get; private set; }
    public int AmountOfVehicle { get; private set; }

    public OperatorListItem(Guid id, DateOnly birth, int licenseAmount, int vehicleAmount)
    {
        Id = id;
        Birth = birth;
        AmountOfLicense = licenseAmount;
        AmountOfVehicle = vehicleAmount;
    }
}