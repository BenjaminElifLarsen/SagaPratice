using Common.Events.Projection;
using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
internal class GenderEventRepository : IGenderEventRepository
{
    private readonly IBaseEventRepository<Guid> _eventRepository;
    private readonly IGenderFactory _factory;

    public GenderEventRepository(IBaseEventRepository<Guid> eventRepository, IGenderFactory factory)
    {
        _eventRepository = eventRepository;
        _factory = factory;
    }

    public void AddEvents(Gender entity)
    {
        var events = new List<Event>();
        foreach (var e in entity.Events)
        { 
            events.Add(e.ConvertToEvent());
        }
        _eventRepository.AddEvents(events);
    }

    public async Task<Gender> GetForOperationAsync(Guid id)
    {
        var events = await _eventRepository.LoadEntityEventsAsync(id, nameof(Gender));
        var entity = _factory.HydrateGender(events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x))));
        return entity;
    }

    public async Task<Gender> GetForOperationAtSpecificPointAsync(Guid id, DateTime timePoint)
    {
        var events = await _eventRepository.LoadEntityEventsUptoAsync(id, nameof(Gender), timePoint);
        var entity = _factory.HydrateGender(events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x))));
        return entity;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(IViewMultiQuery<TProjection> query) where TProjection : IMultiProjection<TProjection>
    {
        var events = await _eventRepository.LoadAllEvents(nameof(Gender));
        var domainEvents = events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x)));
        return domainEvents.ProjectionMultiple(query);
    }

    public async Task<TProjection> GetAsync<TProjection>(Guid id, IViewSingleQuery<TProjection> query) where TProjection : ISingleProjection<TProjection>
    {
        var events = await _eventRepository.LoadEntityEventsAsync(id, nameof(Gender));
        var domainEvents = events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x)));
        return domainEvents.ProjectionSingle(query);
    }
}

