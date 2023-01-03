using Common.Events.Bus;
using Common.Events.Store.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
public abstract class RecogniseProcessManager : BaseProcessManager,
    IAppDomainEventHandler<GenderRecognisedSucceeded>,
    IAppSystemEventHandler<GenderRecognisedFailed>
{
    public abstract void Handler(GenderRecognisedFailed @event);

    public abstract void Handler(GenderRecognisedSucceeded @event);
}