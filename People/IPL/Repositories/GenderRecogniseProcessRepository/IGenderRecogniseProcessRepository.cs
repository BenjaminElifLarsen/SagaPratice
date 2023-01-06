using PeopleDomain.AL.ProcessManagers.Gender.Recognise;

namespace PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;
public interface IGenderRecogniseProcessRepository
{
    public Task<GenderRecogniseProcessManager> LoadAsync(Guid correlationId);
    public void Save(GenderRecogniseProcessManager manager);
    public void Delete(Guid correlationId);
}
