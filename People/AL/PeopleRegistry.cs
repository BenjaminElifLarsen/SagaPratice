using Common.Routing;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.AL.Handlers.Event;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL;
public class PeopleRegistry : IRoutingRegistry
{
	private readonly IPeopleCommandBus _commandBus;
	private readonly IPeopleDomainEventBus _eventBus;
	private readonly IPeopleCommandHandler _commandHandler;
	private readonly IPeopleEventHandler _eventHandler;

	public PeopleRegistry(IPeopleCommandBus commandBus, IPeopleDomainEventBus eventBus, IPeopleCommandHandler commandHandler, IPeopleEventHandler eventHandler)
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
        _eventBus.RegisterHandler<PersonFired>(_eventHandler.Handle); //need to registrate the process manager, figure out best way, e.g. each process manager registrate themselv at the event bus
        //_eventBus.RegisterHandler<PersonChangedGender>(_eventHandler.Handle);
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

	public void SetUpRouting(IPersonalInformationChangeProcessManager processManager)
	{
		_eventBus.RegisterHandler<PersonPersonalInformationChangedSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonPersonalInformationChangedFailed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonAddedToGenderSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonChangedGender>(processManager.Handler);
    }
}
