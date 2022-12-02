using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
public interface IUnrecogniseProcessManager : IProcessManager,
    IProcessManagerEventHandler<GenderUnrecognisedSuccessed>,
    IProcessManagerEventHandler<GenderUnrecognisedFailed>
{
}
