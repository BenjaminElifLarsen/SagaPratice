using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Hire;
public interface IHireProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonHired>,
    IProcessManagerEventHandler<PersonAddedToGenderSuccessed>
{
}
