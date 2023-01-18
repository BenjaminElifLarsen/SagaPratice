using PersonDomain.AL.ProcessManagers.Person.Fire;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
public interface IPersonFireProcessRepository
{
    public Task<FireProcessManager> LoadAsync(Guid correlationId);
    public void Save(FireProcessManager manager);
    public void Delete(Guid correlationId);
}
