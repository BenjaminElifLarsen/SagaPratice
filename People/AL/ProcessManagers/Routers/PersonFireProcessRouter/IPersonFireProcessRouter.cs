using Common.Events.Bus;
using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;

public interface IPersonFireProcessRouter : IProcessManagerRouter,
    IEventHandler<PersonFiredSucceeded>,
    IEventHandler<PersonFiredFailed>,
    IEventHandler<PersonRemovedFromGenderSucceeded>,
    IEventHandler<PersonRemovedFromGenderFailed>
{
}