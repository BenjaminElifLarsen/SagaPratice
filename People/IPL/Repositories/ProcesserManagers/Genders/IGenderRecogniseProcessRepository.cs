using PersonDomain.AL.ProcessManagers.Gender.Recognise;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.Genders;
public interface IGenderRecogniseProcessRepository
{
    public Task<GenderRecogniseProcessManager> LoadAsync(Guid correlationId);
    public void Save(GenderRecogniseProcessManager manager);
    public void Delete(Guid correlationId);
}
