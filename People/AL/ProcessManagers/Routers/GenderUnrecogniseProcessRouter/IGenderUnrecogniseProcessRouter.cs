using Common.Events.Bus;
using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
public interface IGenderUnrecogniseProcessRouter : IProcessManagerRouter,
    IEventHandler<GenderUnrecognisedSucceeded>,
    IEventHandler<GenderUnrecognisedFailed>
{
}
