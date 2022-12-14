using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.Handlers.Command;
using PersonDomain.AL.ProcessManagers.Gender.Recognise;
using PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.Services.Genders;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.Registries;
public sealed class PersonRegistry : IPersonRegistry
{
    private readonly IPersonCommandBus _commandBus;
    private readonly IPersonDomainEventBus _eventBus;
    private readonly IPersonCommandHandler _commandHandler;

    public PersonRegistry(IPersonCommandBus commandBus, IPersonDomainEventBus eventBus, IPersonCommandHandler commandHandler)
    {
        _commandBus = commandBus;
        _eventBus = eventBus;
        _commandHandler = commandHandler; //consider some way to set it up such than a domain event gets converted to an intergration event and placed on an intergration handler
    } //so an intergration bus that registrates domain events, converts them to intergration events and publish them to its handlers

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
        _eventBus.RegisterHandler<PersonPersonalInformationChangedSuccessed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonPersonalInformationChangedFailed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonChangedGender>(processManager.Handle);
    }

    public void SetUpRouting(IFireProcessManager processManager)
    {
        _eventBus.RegisterHandler<PersonFiredSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonFiredFailed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processManager.Handle);
    }

    public void SetUpRouting(IHireProcessManager processManager)
    {
        _eventBus.RegisterHandler<PersonHiredSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonHiredFailed>(processManager.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processManager.Handle);
    }

    //public void SetUpRouting(IRecogniseProcessManager processManager)
    //{
    //    _eventBus.RegisterHandler<GenderRecognisedSucceeded>(processManager.Handler);
    //    _eventBus.RegisterHandler<GenderRecognisedFailed>(processManager.Handler);
    //}

    public void SetUpRouting(IUnrecogniseProcessManager processManager)
    {
        _eventBus.RegisterHandler<GenderUnrecognisedSucceeded>(processManager.Handle);
        _eventBus.RegisterHandler<GenderUnrecognisedFailed>(processManager.Handle);
    }

    public void SetUpRouting(IGenderRecogniseProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<GenderRecognisedSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<GenderRecognisedFailed>(processRouter.Handle);
    }

    public void SetUpRouting(IGenderService service)
    {
        _eventBus.RegisterHandler<RecognisedSucceeded>(service.Handle);
        _eventBus.RegisterHandler<RecognisedFailed>(service.Handle);
    }
}
