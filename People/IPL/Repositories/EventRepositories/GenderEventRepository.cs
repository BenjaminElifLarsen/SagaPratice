using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories;
internal class GenderEventRepository : IGenderEventRepository
{
    private readonly IBaseEventRepository<Guid> _eventRepository;
    private readonly IGenderFactory _factory;

    public void AddEvents(Gender entity)
    {
        var events = new List<Event>();
        foreach(var e in entity.Events)
        { //consider a good way and where to convert the data into the int/property combinations
            if(e is GenderRecognisedSucceeded gr)
            {
                events.Add(new Event(e, GenderConversion.Get(gr), EventType.Create));
            }
            else if(e is GenderUnrecognisedSucceeded gu)
            {
                events.Add(new Event(e, GenderConversion.Get(gu), EventType.Remove));
            }
            else if(e is PersonAddedToGenderSucceeded pa)
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

    public async Task<Gender> GetGenderAsync(Guid id)
    {
        var events = await _eventRepository.LoadEntityEventsAsync(id);
        var entity = _factory.HydrateGender(events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x))));
        return entity;
    }

    public async Task<Gender> GetGenderAtSpecificPointAsync(Guid id, DateTime timePoint)
    {
        var events = await _eventRepository.LoadEntityEventsUptoAsync(id, timePoint);
        var entity = _factory.HydrateGender(events.Select(x => GenderConversion.Set(Event.EventFromGeneric(x))));
        return entity;
    }
}

