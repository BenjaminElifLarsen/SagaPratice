﻿using Common.CQRS.Commands;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPeopleCommandBus _commandBus;

	public PeopleService(IUnitOfWork unitOfWork, IPeopleCommandBus commandBus)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
	}
}
