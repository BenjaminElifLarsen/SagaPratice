using Common.RepositoryPattern.Events;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
internal class PersonEventRepository : IPersonEventRepository
{
    private readonly IBaseEventRepository<Guid> _eventRepository;
    public PersonEventRepository(IBaseEventRepository<Guid> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public void AddEvents(Person entity)
    {
        throw new NotImplementedException();
    }

    public Task<Person> GetForOperationAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
