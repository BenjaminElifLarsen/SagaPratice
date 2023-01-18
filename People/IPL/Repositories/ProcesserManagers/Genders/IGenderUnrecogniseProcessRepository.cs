using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.Genders;
public interface IGenderUnrecogniseProcessRepository
{
    public Task<GenderUnrecogniseProcessManager> LoadAsync(Guid correlationId);
    public void Save(GenderUnrecogniseProcessManager manager);
    public void Delete(Guid correlationId);
}
