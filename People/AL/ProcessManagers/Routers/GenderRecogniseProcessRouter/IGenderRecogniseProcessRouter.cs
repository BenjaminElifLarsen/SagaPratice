using Common.Events.Bus;
using Common.Events.Store.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
public interface IGenderRecogniseProcessRouter : IProcessManagerRouter,
    IEventHandler<GenderRecognisedSucceeded>,
    IEventHandler<GenderRecognisedFailed>
{
}
