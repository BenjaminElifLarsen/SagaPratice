using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
public interface IPersonEventRepository
{
    public Task<Person> GetForOperationAsync(Guid id);
    public void AddEvents(Person entity);
}
