using BaseRepository;
using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;
using PersonDomain.DL.CQRS.Queries;
using PersonDomain.DL.CQRS.Queries.Events;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;
using System.Linq;

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
        { //consider a good way and where to convert the data into the int/property combinations
            if (e is GenderRecognisedSucceeded gr)
            {
                events.Add(new Event(e, GenderConversion.Get(gr), EventType.Create));
            }
            else if (e is GenderUnrecognisedSucceeded gu)
            {
                events.Add(new Event(e, GenderConversion.Get(gu), EventType.Remove));
            }
            else if (e is PersonAddedToGenderSucceeded pa)
            {
                events.Add(new Event(e, GenderConversion.Get(pa), EventType.Modify));
            }
            else if (e is PersonRemovedFromGenderSucceeded pr)
            {
                events.Add(new Event(e, GenderConversion.Get(pr), EventType.Modify));
            }
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

    public GenderSubject TestDeleteLater(Guid id)
    {
        var events = _eventRepository.LoadEntityEventsAsync(id, nameof(Gender)).Result;
        return GenderSubject.Projection(events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x))));
        //GenderSubject test = null;
        //(_eventRepository as MockEventRepository<Guid, IEventStore<Guid>>).TestDeleteWhenDone
        //    .AsQueryable()
        //    .Where(x => x.AggregateId == id)
        //    .Select(new GenderSubjectQuery().Projection());
        //return test; //might be best to get all the events and then handle them in the backend rather than in the context
        ////so get the events, convert them, and then project them into the new state. 
    }
}

