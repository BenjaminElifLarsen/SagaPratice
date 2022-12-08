using Common.Events.Domain;
using System.Diagnostics;

namespace VehicleDomain.AL.Busses.Event;
internal class MockDomainEventBus : IVehicleDomainEventBus
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

        if (!handlers.Select(x => { dynamic d = x.Target; return d; }).Any(x => x.handler.Target == handler.Target && x.handler.Method == handler.Method))
            handlers.Add(x => handler((T)x));

    }

    public void Publish<T>(T @event) where T : IDomainEvent
    {
        List<Action<IDomainEvent>> handlers;
        #if (DEBUG)
        Debug.WriteLine($"{@event.CorrelationId} : {@event.CausationId} : {@event.EventId} : {@event.GetType()}");
        #else
            //write to log
        #endif
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

        var toRemove = handlers.Select(hdlr => { dynamic d = hdlr.Target; return (d, hdlr); }).SingleOrDefault(x => x.d.handler.Target == handler.Target && x.d.handler.Method == handler.Method).hdlr;
        if (toRemove is not null)
        {
            handlers.Remove(toRemove);
        }
    }
}
