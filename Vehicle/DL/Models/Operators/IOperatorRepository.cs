using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators;
public interface IOperatorRepository
{
    Task<bool> IsIdUniqueAsync(Guid id);
    Task<TProjection> GetAsync<TProjection>(Guid id, BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel;
    void Create(Operator entity);
    void Update(Operator entity);
    void Delete(Operator entity);
    Task<Operator> GetForOperationAsync(Guid id);
}
