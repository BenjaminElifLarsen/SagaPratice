using Common.CQRS.Commands;
using Common.ResultPattern;

namespace PeopleDomain.AL.Busses.Command;
internal class MockCommandBus : IPeopleCommandBus
{
    private readonly Dictionary<Type, List<Func<ICommand, Result>>> _routes;

    public MockCommandBus() 
    {
        _routes = new();
    }

    public Result Publish<T>(T command) where T : ICommand
    {
        List<Func<ICommand, Result>> handlers;

        if (!_routes.TryGetValue(command.GetType(), out handlers))
            return new SuccessResultNoData();

        if (handlers.Count > 1)
            throw new Exception("To many command handlers.");

        return handlers[0](command);
    }

    public void RegisterHandler<T>(Func<T, Result> handler) where T : ICommand
    {
        List<Func<ICommand, Result>> handlers;

        if(!_routes.TryGetValue(typeof(T), out handlers))
        {
            handlers = new();
            _routes.Add(typeof(T), handlers);
        }

        if (handlers.Any())
            throw new Exception("Cannot add more handlers. Commands can only have a single handler.");

        handlers.Add(x => handler((T)x));
    }

    public void UnregisterHandler<T>(Func<T, Result> handler) where T : ICommand
    {
        List<Func<ICommand, Result>> handlers;

        if (!_routes.TryGetValue(typeof(T), out handlers))
            return;

        var toRemove = handlers.SingleOrDefault(x => handler((T)x));
        if(toRemove is not null)
        {
            handlers.Remove(toRemove);
        }
    }
}
