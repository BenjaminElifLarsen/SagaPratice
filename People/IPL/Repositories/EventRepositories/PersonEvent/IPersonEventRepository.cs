using Common.Events.Projection;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
public interface IPersonEventRepository
{
    public Task<Person> GetForOperationAsync(Guid id);
    public void AddEvents(Person entity);
    public Task<TProjection> GetAsync<TProjection>(Guid id, IViewSingleQuery<TProjection> query) where TProjection : ISingleProjection<TProjection>;
    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(IViewMultiQuery<TProjection> query) where TProjection : IMultiProjection<TProjection>;
}
