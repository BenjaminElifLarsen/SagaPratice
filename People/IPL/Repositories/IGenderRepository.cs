using Common.CQRS.Queries;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
internal interface IGenderRepository
{
    void Recognise(Gender entity);
    void Unrecognise(Gender entity);
    void Save();
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Gender, TProjection> query) where TProjection : BaseReadModel;
}
