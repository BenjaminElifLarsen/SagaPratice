using Common.Events.Projection;
using PersonDomain.DL.CQRS.Queries.Events;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
public interface IPersonEventRepository
{
    public Task<Person> GetForOperationAsync(Guid id);
    public void AddEvents(Person entity);
    public T Test<T>(Guid id, IViewQuery<T> query) where T : IProjection;
}
