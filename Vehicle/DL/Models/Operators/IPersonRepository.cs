using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators;
public interface IPersonRepository
{
    Task<bool> IsIdUniqueAsync(int id);
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel;
    void Save();
    void Create(Operator entity);
    void Update(Operator entity);
    void Delete(Operator entity);
    Task<Operator> GetForOperationAsync(int id);
}
