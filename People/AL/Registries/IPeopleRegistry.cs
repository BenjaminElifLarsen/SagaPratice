using Common.Routing;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;
using PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
using PeopleDomain.AL.ProcessManagers.Person.Fire;
using PeopleDomain.AL.ProcessManagers.Person.Hire;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;

namespace PeopleDomain.AL.Registries;
public interface IPeopleRegistry : IRoutingRegistry
{
    public void SetUpRouting(IPersonalInformationChangeProcessManager processManager);
    public void SetUpRouting(IFireProcessManager processManager);
    public void SetUpRouting(IHireProcessManager processManager);
    //public void SetUpRouting(IRecogniseProcessManager processManager);
    public void SetUpRouting(IUnrecogniseProcessManager processManager);
}
