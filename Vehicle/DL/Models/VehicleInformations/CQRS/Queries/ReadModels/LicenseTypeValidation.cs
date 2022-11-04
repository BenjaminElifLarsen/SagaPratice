﻿using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries.ReadModels;
internal record LicenseTypeValidation : BaseReadModel
{
    public int Id { get; private set; }

    public LicenseTypeValidation(int id)
    {
        Id = id;
    }
}
