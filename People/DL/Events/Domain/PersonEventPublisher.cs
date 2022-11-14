using Common.Events.Domain;

namespace PeopleDomain.DL.Events.Domain;
internal class PersonEventPublisher : IPersonEventPublisher //when moving the interface to Common rename to IBaseBus or something like that
{ //works kind as a bus currently, a fake bus, but kind of following the pricipels.
    private readonly Dictionary<Type, List<Action<IDomainEvent>>> _routes;

	public PersonEventPublisher()
	{
		_routes = new();
	}

	public void RegisterHandler<T>(Action<T> handler) where T : IDomainEvent
	{
		List<Action<IDomainEvent>> handlers;

		if(!_routes.TryGetValue(typeof(T), out handlers))
		{
			handlers = new();
			_routes.Add(typeof(T), handlers);
		}

		handlers.Add(x => handler((T)x));
	}

	public void Publish<T>(T @event) where T : IDomainEvent
	{
		List<Action<IDomainEvent>> handlers;
		
		if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;

		foreach(var handler in handlers)
		{
			handler(@event);
		}
	}

	public void UnregisterHandler<T>(Action<T> handler) where T : IDomainEvent
	{
        List<Action<IDomainEvent>> handlers;

		if (!_routes.TryGetValue(typeof(T), out handlers)) return;

        handlers.Remove(x => handler((T)x));
    }
}
