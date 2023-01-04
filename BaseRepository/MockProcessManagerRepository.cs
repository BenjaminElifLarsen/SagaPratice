using Common.Events.Store.ProcessManager;
using Common.RepositoryPattern.ProcessManagers;

namespace BaseRepository;
public class MockProcessManagerRepository<TProcessManager> : IBaseProcessManagerRepository<TProcessManager> where TProcessManager : BaseProcessManager
{
    private static HashSet<TProcessManager> _processManagers; //move over to a context file when closer to finalise the new design regarding its implemntation.

    public MockProcessManagerRepository()
    {
        _processManagers = new();
    }

    public void Delete(Guid correlationId)
    {
        _processManagers.Remove(_processManagers.SingleOrDefault(x => x.CorrelationId == correlationId));
    }

    public async Task<TProcessManager> LoadAsync(Guid correlationId)
    {
        return await Task.Run(() => _processManagers.SingleOrDefault(x => x.CorrelationId == correlationId));
    }

    public void Save(TProcessManager processManager)
    {
        if (!_processManagers.Contains(processManager))
        {
            _processManagers.Add(processManager);
        }
    }
}
