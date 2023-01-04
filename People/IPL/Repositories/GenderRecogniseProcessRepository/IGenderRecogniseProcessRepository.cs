using PeopleDomain.AL.ProcessManagers.Gender.Recognise;

namespace PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;
public interface IGenderRecogniseProcessRepository
{
    public Task<RecogniseProcessManager> LoadAsync(Guid correlationId);
    public void Save(RecogniseProcessManager manager);
    public void Delete(Guid correlationId);
}
