using Common.CQRS.Commands;
using Common.Events.Base;
using Common.Events.Bus;
using Common.Events.Store.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
public abstract class RecogniseProcessManager : BaseProcessManager,
    IEventHandler<GenderRecognisedSucceeded>,
    IAppSystemEventHandler<GenderRecognisedFailed>
{
    public IEnumerable<ICommand> Commands { get; }

    public IEnumerable<IBaseEvent> Events { get; }

    public abstract void Handle(GenderRecognisedFailed @event);

    public abstract void Handle(GenderRecognisedSucceeded @event);
}