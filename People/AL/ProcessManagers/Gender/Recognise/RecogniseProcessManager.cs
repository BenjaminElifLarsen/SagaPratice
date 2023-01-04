using Common.CQRS.Commands;
using Common.Events.Base;
using Common.Events.Bus;
using Common.Events.Store.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
public abstract class RecogniseProcessManager : BaseProcessManager,
    IAppDomainEventHandler<GenderRecognisedSucceeded>,
    IAppSystemEventHandler<GenderRecognisedFailed>
{
    public IEnumerable<ICommand> Commands { get; }

    public IEnumerable<IBaseEvent> Events { get; }

    public abstract void Handler(GenderRecognisedFailed @event);

    public abstract void Handler(GenderRecognisedSucceeded @event);
}