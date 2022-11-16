using Common.CQRS.Queries;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
public interface IGenderRepository
{
    void Recognise(Gender entity);
    void Unrecognise(Gender entity);
    void Update(Gender entity);
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel;
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel;
    Task<Gender> GetForOperationAsync(int id);
}
