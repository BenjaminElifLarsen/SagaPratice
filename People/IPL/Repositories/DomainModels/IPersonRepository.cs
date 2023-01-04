using Common.CQRS.Queries;
using PeopleDomain.DL.Models;

namespace PeopleDomain.IPL.Repositories.DomainModels;
public interface IPersonRepository
{
    public void Hire(Person entity);
    public void Fire(Person entity);
    public void UpdatePersonalInformation(Person entity);
    public Task<bool> DoesPersonExist(int id);
    Task<Person> GetForOperationAsync(int id);
    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel;
    public Task<TProjection> GetAsync<TProjection>(int id, BaseQuery<Person, TProjection> query) where TProjection : BaseReadModel;
}
