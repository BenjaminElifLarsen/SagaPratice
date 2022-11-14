using Common.CQRS.Queries;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
public interface IPersonRepository
{
    public void Hire(Person entity);
    public void Fire(Person entity);
    public void UpdatePersonalInformation(Person entity);
    public void Save();
    public Task<bool> DoesPersonExist(int id);
    Task<Person> GetForOperationAsync(int id);
    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel;
    public Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel;
    Task<IEnumerable<Person>> AllForOperationsAsync();
}
