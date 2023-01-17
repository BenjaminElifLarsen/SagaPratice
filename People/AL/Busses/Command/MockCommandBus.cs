using Common.CQRS.Commands;
using System.Diagnostics;

namespace PersonDomain.AL.Busses.Command;
internal sealed class MockCommandBus : IPersonCommandBus
{
    private readonly Dictionary<Type, List<Action<ICommand>>> _routes;

    public MockCommandBus() 
    {
        _routes = new();
    }

    public void Dispatch<T>(T command) where T : ICommand
    {
        List<Action<ICommand>> handlers;
        #if(DEBUG)
            Debug.WriteLine($"{command.CorrelationId} : {command.CausationId} : {command.CommandId} : {command.GetType()}");
        #else
            //write to log
        #endif
        if (!_routes.TryGetValue(command.GetType(), out handlers))
            return;// new SuccessResultNoData();

        if (handlers.Count > 1)
            throw new Exception("To many command handlers.");
        handlers[0](command);
        return; 
    }

    public void RegisterHandler<T>(Action<T> handler) where T : ICommand
    {
        List<Action<ICommand>> handlers;

        if(!_routes.TryGetValue(typeof(T), out handlers))
        {
            handlers = new();
            _routes.Add(typeof(T), handlers);
        }

        if (handlers.Any())
            throw new Exception("Cannot add more handlers. Commands can only have a single handler.");

        handlers.Add(x => handler((T)x));
    }

    public void UnregisterHandler<T>(Action<T> handler) where T : ICommand
    {
        List<Action<ICommand>> handlers;

        if (!_routes.TryGetValue(typeof(T), out handlers))
            return;

        var toRemove = handlers.SingleOrDefault(x => handler((T)x));
        if(toRemove is not null)
        {
            handlers.Remove(toRemove);
        }
    }
}
