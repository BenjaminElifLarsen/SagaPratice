using Common.Events.Bus;
using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
public interface IRecogniseProcessManager : IProcessManager,
    IAppDomainEventHandler<GenderRecognisedSucceeded>,
    IProcessManagerEventHandler<GenderRecognisedFailed>
{
}
