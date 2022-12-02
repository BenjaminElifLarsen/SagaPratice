using Common.Events.Domain;
using Common.RepositoryPattern;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
}
