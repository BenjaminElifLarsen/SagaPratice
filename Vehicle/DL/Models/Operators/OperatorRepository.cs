using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Operators;
internal class OperatorRepository : IOperatorRepository
{
    private readonly IBaseRepository<Operator> _baseRepository;

    public OperatorRepository(IBaseRepository<Operator> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.AllAsync(query);
    }

    public void Create(Operator entity)
    {
        _baseRepository.Create(entity);
    }

    public void Delete(Operator entity)
    {
        _baseRepository.Delete(entity);
    }

    public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Operator, TProjection> query) where TProjection : BaseReadModel
    {
        return await _baseRepository.FindByPredicateAsync(x => x.OperatorId == id, query);
    }

    public async Task<Operator> GetForOperationAsync(int id)
    {
        return await _baseRepository.FindByPredicateForOperationAsync(x => x.OperatorId == id);
    }

    public async Task<bool> IsIdUniqueAsync(int id)
    {
        return await _baseRepository.IsUniqueAsync(x => x.OperatorId == id);
    }

    public void Save()
    {
        throw new NotImplementedException();//_baseRepository.SaveChanges();
    }

    public void Update(Operator entity)
    {
        _baseRepository.Update(entity);
    }
}
