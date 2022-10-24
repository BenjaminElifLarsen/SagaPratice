using Common.CQRS.Queries;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.People;
public class PersonRepository : IPersonRepository
{
    private readonly IBaseRepository<Person> _baseRepository;

	public PersonRepository(IBaseRepository<Person> baseRepository)
	{
		_baseRepository = baseRepository;
	}

	public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel
	{
		return await _baseRepository.AllAsync(query);
	}

	public void Create(Person entity)
	{
		_baseRepository.Create(entity);
	}

	public void Delete(Person entity)
	{
		_baseRepository.Delete(entity);
	}

	public async Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel
	{
		return await _baseRepository.FindByPredicateAsync(x => x.PersonId == id, query);
	}

	public async Task<Person> GetForOperationAsync(int id)
	{
		return await _baseRepository.FindByPredicateForOperationAsync(x => x.PersonId == id);
	}

	public async Task<bool> IsIdUniqueAsync(int id)
	{
		return await _baseRepository.IsUniqueAsync(x => x.PersonId == id);
	}

	public void Save()
	{
		_baseRepository.SaveChanges();
	}

	public void Update(Person entity)
	{
		_baseRepository.Update(entity);
	}
}
