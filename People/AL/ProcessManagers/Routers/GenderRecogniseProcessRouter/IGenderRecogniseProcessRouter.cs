using Common.Events.Bus;
using Common.Events.Store.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
public interface IGenderRecogniseProcessRouter : IProcessManagerRouter,
    IEventHandler<GenderRecognisedSucceeded>,
    IEventHandler<GenderRecognisedFailed>
{
}
