using Common.CQRS.Commands;
using Common.Events.State;
using Common.Events.Store.Event;
using Common.Events.Store.ProcessManager;

namespace PersonDomain.AL.ProcessManagers;
public class BaseProcessManager : IBaseProcessManager
{
    private readonly List<string> _errors;
    private readonly List<ICommand> _commands;
    private readonly List<StateEvent> _events;
    public virtual IEnumerable<ICommand> Commands { get => _commands; }
    public virtual IEnumerable<StateEvent> StateEvents { get => _events; }
    public virtual Guid ProcessManagerId { get; protected set; }
    public virtual Guid CorrelationId { get; protected set; }
    public virtual Guid Versioning { get; protected set; }
    public virtual IEnumerable<string> Errors => _errors;

    protected BaseProcessManager()
    {
        _errors = new();
        _commands = new();
        _events = new();
    }

    internal BaseProcessManager(Guid correlationId) : this()
    {
        CorrelationId = correlationId;
    }

    public virtual void GenerateNewVersioning()
    {
        Versioning = Guid.NewGuid();
    }
    protected void AddStateEvent(StateEvent @event) => _events.Add(@event);

    protected void AddCommand(ICommand command) => _commands.Add(command);

    protected void AddErrors(IEnumerable<string> errors) => _errors.AddRange(errors);
}
