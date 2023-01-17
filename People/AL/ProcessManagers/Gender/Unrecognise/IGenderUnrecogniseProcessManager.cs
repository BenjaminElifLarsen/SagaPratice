using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
public interface IGenderUnrecogniseProcessManager : IBaseProcessManager,
    IProcessManagerEventHandler<GenderUnrecognisedSucceeded>,
    IProcessManagerEventHandler<GenderUnrecognisedFailed>
{
}
