using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
public interface IUnrecogniseProcessManager : IProcessManager,
    IProcessManagerEventHandler<GenderUnrecognisedSucceeded>,
    IProcessManagerEventHandler<GenderUnrecognisedFailed>
{
}
