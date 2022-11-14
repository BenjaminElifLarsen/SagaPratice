using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.IPL.Services;
public interface IUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public void Save();
}
