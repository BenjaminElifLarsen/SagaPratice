using Common.Events.Bus;
using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
public interface IPersonHireProcessRouter : IProcessManagerRouter,
    IEventHandler<PersonHiredSucceeded>,
    IEventHandler<PersonHiredFailed>,
    IEventHandler<PersonAddedToGenderSucceeded>,
    IEventHandler<PersonAddedToGenderFailed>
{
}
