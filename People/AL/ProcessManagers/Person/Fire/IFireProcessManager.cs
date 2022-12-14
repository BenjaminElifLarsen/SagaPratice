using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Fire;
public interface IFireProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonFiredSucceeded>,
    IProcessManagerEventHandler<PersonFiredFailed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderSucceeded>,
    IProcessManagerEventHandler<PersonRemovedFromGenderFailed>
{
}
