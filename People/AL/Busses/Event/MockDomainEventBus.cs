﻿using Common.Events.Domain;

namespace PeopleDomain.AL.Busses.Event;
internal class MockDomainEventBus : IPeopleDomainEventBus
{ //works kind as a bus currently, a fake bus, but kind of following the pricipels, but does not permit comminucation between different modules.
  //the integration event bus would be closer to an actually bus as it would handle communication over different modules.
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

        //var test = handlers.SingleOrDefault(x => handler((T)x)); 
        //if (!handlers.Any(x => x == test)) //from testing this approach does not work. It is true if any handler of T is present, it does not require to be the exactly same method
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

        var toRemove = handlers.SingleOrDefault(x => handler((T)x)); //from the knowledge learned from testbed and register handler this approach will not work.
        if (toRemove is not null)
        {
            handlers.Remove(toRemove);
        }
        throw new NotImplementedException();
    }
}
