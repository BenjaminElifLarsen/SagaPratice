using Common.RepositoryPattern;
using PeopleDomain.IPL.Repositories.DomainModels;
using PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;

namespace PeopleDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository { get; }
}
