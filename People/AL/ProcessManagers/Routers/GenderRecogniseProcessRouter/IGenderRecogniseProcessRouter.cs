using Common.Events.Bus;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
public interface IGenderRecogniseProcessRouter : 
    IEventHandler<GenderRecognisedSucceeded>,
    IEventHandler<GenderRecognisedFailed>
{
}
