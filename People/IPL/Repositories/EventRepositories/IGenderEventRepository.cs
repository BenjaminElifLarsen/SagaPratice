using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories;
public interface IGenderEventRepository
{
    public Task<Gender> GetGenderAsync(Guid id);
    public Task<Gender> GetGenderAtSpecificPointAsync(Guid id, DateTime timePoint);
    public void AddEvents(Gender entity);
}