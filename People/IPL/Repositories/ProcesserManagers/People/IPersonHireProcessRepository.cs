using PersonDomain.AL.ProcessManagers.Person.Hire;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
public interface IPersonHireProcessRepository
{
    public Task<HireProcessManager> LoadAsync(Guid correlationId);
    public void Save(HireProcessManager manager);
    public void Delete(Guid correlationId);
}
