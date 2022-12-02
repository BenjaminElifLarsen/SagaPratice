using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;
using PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
using PeopleDomain.AL.ProcessManagers.Person.Fire;
using PeopleDomain.AL.ProcessManagers.Person.Hire;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Registries;
public class PeopleRegistry : IPeopleRegistry
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly IPeopleDomainEventBus _eventBus;
    private readonly IPeopleCommandHandler _commandHandler;

    public PeopleRegistry(IPeopleCommandBus commandBus, IPeopleDomainEventBus eventBus, IPeopleCommandHandler commandHandler)
    {
        _commandBus = commandBus;
        _eventBus = eventBus;
        _commandHandler = commandHandler;
    }

    public void SetUpRouting()
    {
        RoutingCommand();
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

    public void SetUpRouting(IFireProcessManager processManager)
    {
        _eventBus.RegisterHandler<PersonFiredSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonFiredFailed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processManager.Handler);
    }

    public void SetUpRouting(IHireProcessManager processManager)
    {
        _eventBus.RegisterHandler<PersonHiredSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonHiredFailed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonAddedToGenderSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processManager.Handler);
    }

    public void SetUpRouting(IRecogniseProcessManager processManager)
    {
        _eventBus.RegisterHandler<GenderRecognisedSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<GenderRecognisedFailed>(processManager.Handler);
    }

    public void SetUpRouting(IUnrecogniseProcessManager processManager)
    {
        _eventBus.RegisterHandler<GenderUnrecognisedSuccessed>(processManager.Handler);
        _eventBus.RegisterHandler<GenderUnrecognisedFailed>(processManager.Handler);
    }
}
