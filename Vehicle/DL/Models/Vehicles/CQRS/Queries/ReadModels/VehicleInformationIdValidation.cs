﻿using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal class VehicleInformationIdValidation : BaseReadModel
{
    public int Id { get; private set; }

	public VehicleInformationIdValidation(int id)
	{
		Id = id;
	}
}