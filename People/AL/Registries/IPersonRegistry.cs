using Common.Routing;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.Services.Genders;

namespace PersonDomain.AL.Registries;
public interface IPersonRegistry : IRoutingRegistry
{
    public void SetUpRouting(IPersonalInformationChangeProcessManager processManager);
    public void SetUpRouting(IFireProcessManager processManager);
    public void SetUpRouting(IHireProcessManager processManager);
    //public void SetUpRouting(IRecogniseProcessManager processManager);
    public void SetUpRouting(IUnrecogniseProcessManager processManager);
    public void SetUpRouting(IGenderRecogniseProcessRouter processRouter);
    public void SetUpRouting(IGenderService service);
}
