using Common.Events.Domain;

namespace VehicleDomain.AL.Busses.Event;
internal class MockDomainEventBus : IDomainEventBus
{
    private readonly Dictionary<Type, List<Action<IDomainEvent>>> _routes;

    public MockDomainEventBus()
    {
        _routes = new();
    }

    public void RegisterHandler<T>(Action<T> handler) where T : IDomainEvent
    {
        List<Action<IDomainEvent>> handlers;

        if (!_routes.TryGetValue(typeof(T), out handlers))
        {
            handlers = new();
            _routes.Add(typeof(T), handlers);
        }

        var test = handlers.SingleOrDefault(x => handler((T)x));
        if (!handlers.Any(x => x == test))
            handlers.Add(x => handler((T)x));

    }

    public void Publish<T>(T @event) where T : IDomainEvent
    {
        List<Action<IDomainEvent>> handlers;

        if (!_routes.TryGetValue(@event.GetType(), out handlers))
            return;

        foreach (var handler in handlers)
        {
            handler(@event);
        }
    }

    public void UnregisterHandler<T>(Action<T> handler) where T : IDomainEvent
    {
        List<Action<IDomainEvent>> handlers;

        if (!_routes.TryGetValue(typeof(T), out handlers))
            return;

        var toRemove = handlers.SingleOrDefault(x => handler((T)x));
        if (toRemove is not null)
        {
            handlers.Remove(toRemove);
        }
    }
}
