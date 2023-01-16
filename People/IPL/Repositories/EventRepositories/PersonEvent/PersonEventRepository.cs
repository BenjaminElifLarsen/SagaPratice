using Common.Events.Projection;
using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
internal class PersonEventRepository : IPersonEventRepository
{
    private readonly IBaseEventRepository<Guid> _eventRepository;
    private readonly IPersonFactory _factory;
    public PersonEventRepository(IBaseEventRepository<Guid> eventRepository, IPersonFactory personFactory)
    {
        _eventRepository = eventRepository;
        _factory = personFactory;
    }

    public void AddEvents(Person entity)
    {
        var events = new List<Event>();
        foreach(var e in entity.Events)
        {
            if(e is PersonHiredSucceeded ph)
            {
                events.Add(new Event(e, PersonConversion.Get(ph), EventType.Create));
            }
            else if(e is PersonFiredSucceeded pf)
            {
                events.Add(new Event(e, PersonConversion.Get(pf), EventType.Remove));
            }
            else if(e is PersonChangedGender pcg)
            {
                events.Add(new Event(e, PersonConversion.Get(pcg), EventType.Modify));
            }
            else if(e is PersonChangedBirth pcb)
            {
                events.Add(new Event(e, PersonConversion.Get(pcb), EventType.Modify));
            }
            else if(e is PersonChangedLastName pcl)
            {
                events.Add(new Event(e, PersonConversion.Get(pcl), EventType.Modify));
            }
            else if(e is PersonChangedFirstName pcf)
            {
                events.Add(new Event(e, PersonConversion.Get(pcf), EventType.Modify));
            }
        }
        _eventRepository.AddEvents(events);
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(IViewMultiQuery<TProjection> query) where TProjection : IMultiProjection<TProjection>
    {
        var events = await _eventRepository.LoadAllEvents(nameof(Person));
        var domainEvents = events.Select(x => PersonConversion.Set(Event.EventFromGeneric(x)));
        return domainEvents.ProjectionMulti(query);
    }

    public Task<TProjection> GetAsync<TProjection>(Guid id, IViewSingleQuery<TProjection> query) where TProjection : ISingleProjection<TProjection>
    {
        throw new NotImplementedException();
    }

    public async Task<Person> GetForOperationAsync(Guid id)
    {
        var events = await _eventRepository.LoadEntityEventsAsync(id, nameof(Person));
        var entity = _factory.HydratePerson(events.Select(x => PersonConversion.Set(Event.EventFromGeneric(x))));
        return entity;
    }

}
