using Common.CQRS.Commands;
using Common.Events.Base;
using Common.Storage;

namespace BaseRepository;
public class CommandAndEventStorage : ICommandAndEventStorage // TODO: should this be accessed through a UoW or by itself?
{
    private List<ICommand> _commands;
    private List<IBaseEvent> _events;

    public CommandAndEventStorage()
    {
        _commands = new List<ICommand>();
        _events = new List<IBaseEvent>();        
    }

    public void Add(ICommand command)
    {
        _commands.Add(command);
    }

    public void Add(IBaseEvent @event)
    { // TODO: check that it is not there already
        _events.Add(@event);
    }

    public bool Process()
    {
        throw new NotImplementedException();
        //process events, split into Domain Events and System Events (system first)
        /*
         * if any err, handle and return false
         */
        //process commands

    }
}
