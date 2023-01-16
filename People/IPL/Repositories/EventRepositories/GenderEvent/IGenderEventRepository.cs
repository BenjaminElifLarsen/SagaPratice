using Common.Events.Projection;
using PersonDomain.DL.CQRS.Queries.Events;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
public interface IGenderEventRepository
{
    public Task<Gender> GetForOperationAsync(Guid id);
    public Task<Gender> GetForOperationAtSpecificPointAsync(Guid id, DateTime timePoint);
    public void AddEvents(Gender entity);
    public Task<TProjection> GetAsync<TProjection>(Guid id, IViewSingleQuery<TProjection> query) where TProjection : ISingleProjection<TProjection>;
    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(IViewMultiQuery<TProjection> query) where TProjection : IMultiProjection<TProjection>;
}