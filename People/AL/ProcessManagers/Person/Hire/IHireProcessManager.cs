using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Person.Hire;
public interface IHireProcessManager : IBaseProcessManager,
    IProcessManagerEventHandler<PersonHiredSucceeded>,
    IProcessManagerEventHandler<PersonHiredFailed>,
    IProcessManagerEventHandler<PersonAddedToGenderSucceeded>,
    IProcessManagerEventHandler<PersonAddedToGenderFailed>
{
}
