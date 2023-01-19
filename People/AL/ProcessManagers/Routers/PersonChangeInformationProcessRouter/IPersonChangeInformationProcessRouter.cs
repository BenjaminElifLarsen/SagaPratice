using Common.Events.Bus;
using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
public interface IPersonChangeInformationProcessRouter : IProcessManagerRouter,
    IEventHandler<PersonPersonalInformationChangedSuccessed>,
    IEventHandler<PersonPersonalInformationChangedFailed>,
    IEventHandler<PersonAddedToGenderSucceeded>,
    IEventHandler<PersonRemovedFromGenderSucceeded>,
    IEventHandler<PersonReplacedGender>,
    IEventHandler<PersonAddedToGenderFailed>,
    IEventHandler<PersonRemovedFromGenderFailed>

{
}
