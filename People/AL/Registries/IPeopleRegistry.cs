using Common.Routing;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;

namespace PeopleDomain.AL.Registries;
public interface IPeopleRegistry : IRoutingRegistry
{
    public void SetUpRouting(IPersonalInformationChangeProcessManager processManager);
}
