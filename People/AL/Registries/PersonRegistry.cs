using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.Handlers.Command;
using PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
using PersonDomain.AL.Services.Genders;
using PersonDomain.AL.Services.People;
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

    public void SetUpRouting(IGenderRecogniseProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<GenderRecognisedSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<GenderRecognisedFailed>(processRouter.Handle);
    }

    public void SetUpRouting(IGenderUnrecogniseProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<GenderUnrecognisedSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<GenderUnrecognisedFailed>(processRouter.Handle);
    }

    public void SetUpRouting(IGenderService service)
    {
        _eventBus.RegisterHandler<RecognisedSucceeded>(service.Handle);
        _eventBus.RegisterHandler<RecognisedFailed>(service.Handle);
        _eventBus.RegisterHandler<UnrecognisedSucceeded>(service.Handle);
        _eventBus.RegisterHandler<UnrecognisedFailed>(service.Handle);
    }

    public void SetUpRouting(IPersonService service)
    {
        _eventBus.RegisterHandler<FiredSucceeded>(service.Handle);
        _eventBus.RegisterHandler<FiredFailed>(service.Handle);
        _eventBus.RegisterHandler<RemovedFromGenderSucceeded>(service.Handle);
        _eventBus.RegisterHandler<RemovedFromGenderFailed>(service.Handle);
        _eventBus.RegisterHandler<HiredSucceeded>(service.Handle);
        _eventBus.RegisterHandler<HiredFailed>(service.Handle);
        _eventBus.RegisterHandler<AddedToGenderSucceeded>(service.Handle);
        _eventBus.RegisterHandler<AddedToGenderFailed>(service.Handle);
        _eventBus.RegisterHandler<GenderReplacedSucceeded>(service.Handle);
        _eventBus.RegisterHandler<GenderReplacedFailed>(service.Handle);
        _eventBus.RegisterHandler<InformationChangedSucceeded>(service.Handle);
        _eventBus.RegisterHandler<InformationChangedFailed>(service.Handle);
    }

    public void SetUpRouting(IPersonFireProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<PersonFiredSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonFiredFailed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processRouter.Handle);
    }

    public void SetUpRouting(IPersonHireProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<PersonHiredSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonHiredFailed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processRouter.Handle);
    }

    public void SetUpRouting(IPersonChangeInformationProcessRouter processRouter)
    {
        _eventBus.RegisterHandler<PersonPersonalInformationChangedSuccessed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonPersonalInformationChangedFailed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonAddedToGenderFailed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderSucceeded>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonRemovedFromGenderFailed>(processRouter.Handle);
        _eventBus.RegisterHandler<PersonReplacedGender>(processRouter.Handle);
    }
}
