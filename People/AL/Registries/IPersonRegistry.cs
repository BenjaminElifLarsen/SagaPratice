using Common.Routing;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
using PersonDomain.AL.Services.Genders;
using PersonDomain.AL.Services.People;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Registries;
public interface IPersonRegistry : IRoutingRegistry
{
    public void SetUpRouting(IGenderRecogniseProcessRouter processRouter);
    public void SetUpRouting(IGenderUnrecogniseProcessRouter processRouter);
    public void SetUpRouting(IPersonFireProcessRouter processRouter);
    public void SetUpRouting(IPersonHireProcessRouter processRouter);
    public void SetUpRouting(IPersonChangeInformationProcessRouter processRouter);
    public void SetUpRouting(IGenderService service);
    public void SetUpRouting(IPersonService service);
    public void SetUpRouting(IUnitOfWork unitOfWork);
}
