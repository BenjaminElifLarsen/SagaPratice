using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Hire;
public interface IHireProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonHiredSuccessed>,
    IProcessManagerEventHandler<PersonHiredFailed>,
    IProcessManagerEventHandler<PersonAddedToGenderSuccessed>,
    IProcessManagerEventHandler<PersonAddedToGenderFailed>
{
}
