using Common.CQRS.Commands;
using Common.Events.Domain;
using Common.Other;
using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.AL.Handlers.Event;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL;
public class PeopleRegistry : IRoutingRegistry
{
	private readonly ICommandBus _commandBus;
	private readonly IDomainEventBus _eventBus;
	private readonly IPeopleCommandHandler _commandHandler;
	private readonly IPeopleEventHandler _eventHandler;

	public PeopleRegistry(ICommandBus commandBus, IDomainEventBus eventBus, IPeopleCommandHandler commandHandler, IPeopleEventHandler eventHandler)
	{
		_commandBus = commandBus;
		_eventBus = eventBus;
		_commandHandler = commandHandler;
		_eventHandler = eventHandler;
	}

	public void SetUpRouting()
	{
		RoutingCommand();
		RoutingEvent();
	}

	private void RoutingEvent()
    {
        _eventBus.RegisterHandler<PersonHired>(_eventHandler.Handle); 
        _eventBus.RegisterHandler<PersonFired>(_eventHandler.Handle);
        _eventBus.RegisterHandler<PersonChangedGender>(_eventHandler.Handle);
    }

	private void RoutingCommand()
	{
		_commandBus.RegisterHandler<AddPersonToGender>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemovePersonFromGender>(_commandHandler.Handle);
        _commandBus.RegisterHandler<ChangePersonGender>(_commandHandler.Handle);
		_commandBus.RegisterHandler<HirePersonFromUser>(_commandHandler.Handle);
		_commandBus.RegisterHandler<FirePersonFromUser>(_commandHandler.Handle);
		_commandBus.RegisterHandler<ChangePersonalInformationFromUser>(_commandHandler.Handle);
		_commandBus.RegisterHandler<RecogniseGender>(_commandHandler.Handle);
		_commandBus.RegisterHandler<UnrecogniseGender>(_commandHandler.Handle);
	}
}
