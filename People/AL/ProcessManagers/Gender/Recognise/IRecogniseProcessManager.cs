using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
public interface IRecogniseProcessManager : IProcessManager,
    IProcessManagerEventHandler<GenderRecognisedSuccessed>,
    IProcessManagerEventHandler<GenderRecognisedFailed>
{
}
