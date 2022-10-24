using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.People;
public interface IPersonRepository
{
    Task<bool> IsIdUniqueAsync(int id);
    Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Person,TProjection> query) where TProjection : BaseReadModel;
    void Save();
    void Create(Person entity);
    void Update(Person entity);
    void Delete(Person entity);
    Task<Person> GetForOperationAsync(int id);



}
