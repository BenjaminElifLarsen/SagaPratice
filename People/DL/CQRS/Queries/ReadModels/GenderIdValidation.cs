﻿using Common.CQRS.Queries;

namespace PeopleDomain.DL.CQRS.Queries.ReadModels;
internal record GenderIdValidation : BaseReadModel
{
    public int Id { get; private set; }

	public GenderIdValidation(int id)
	{
		Id = id;
	}
}
