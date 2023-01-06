using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Person.Fire;
public interface IFireProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonFiredSucceeded>,
    IProcessManagerEventHandler<PersonFiredFailed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderSucceeded>,
    IProcessManagerEventHandler<PersonRemovedFromGenderFailed>
{
}
