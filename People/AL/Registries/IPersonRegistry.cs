using Common.Routing;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.Services.Genders;
using PersonDomain.AL.Services.People;

namespace PersonDomain.AL.Registries;
public interface IPersonRegistry : IRoutingRegistry
{
    public void SetUpRouting(IPersonalInformationChangeProcessManager processManager);
    public void SetUpRouting(IHireProcessManager processManager);
    public void SetUpRouting(IGenderRecogniseProcessRouter processRouter);
    public void SetUpRouting(IGenderUnrecogniseProcessRouter processRouter);
    public void SetUpRouting(IPersonFireProcessRouter processRouter);
    public void SetUpRouting(IGenderService service);
    public void SetUpRouting(IPersonService service);
}
