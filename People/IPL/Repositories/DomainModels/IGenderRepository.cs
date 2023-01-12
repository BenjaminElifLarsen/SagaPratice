using Common.CQRS.Queries;
using PersonDomain.DL.CQRS.Queries.Events.ReadModels;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.DomainModels;
public interface IGenderRepository
{
    void Recognise(Gender entity);
    void Unrecognise(Gender entity);
    void Update(Gender entity);
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel;
    Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel;
    Task<Gender> GetForOperationAsync(Guid id);
}
