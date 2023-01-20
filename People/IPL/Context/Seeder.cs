using Common.ProcessManager;
using Common.Routing;
using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
using PersonDomain.AL.Registries;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Services;

namespace PersonDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(IUnitOfWork unitOfWork, IPersonCommandBus commandBus, IEnumerable<IRoutingRegistry> registries, IEnumerable<IProcessManagerRouter> pmRoutes)
    {
        var selected = registries.Single(x => x is IPersonRegistry) as IPersonRegistry;
        selected.SetUpRouting();

        selected.SetUpRouting(pmRoutes.Single(x => x is IGenderRecogniseProcessRouter) as IGenderRecogniseProcessRouter);
        commandBus.Dispatch(new RecogniseGender("She", "Her"));
        commandBus.Dispatch(new RecogniseGender("She", "Them"));
        commandBus.Dispatch(new RecogniseGender("He", "Him"));
        commandBus.Dispatch(new RecogniseGender("They", "Them"));

        var genders = unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result.ToArray();
        selected.SetUpRouting(pmRoutes.Single(x => x is IPersonHireProcessRouter) as IPersonHireProcessRouter);
        commandBus.Dispatch(new HirePersonFromUser("Triss", "Nib", genders.First().Id, new(1986, 1, 2)));
        commandBus.Dispatch(new HirePersonFromUser("Aida", "Nib", genders.First().Id, new(1992, 3, 12)));

        selected.SetUpRouting(pmRoutes.Single(x => x is IPersonChangeInformationProcessRouter) as IPersonChangeInformationProcessRouter);
        var personId = unitOfWork.PersonRepository.AllAsync(new PersonListItemQuery()).Result.Last().Id;

        commandBus.Dispatch(new ChangePersonalInformationFromUser(personId, null, null, null, genders[1].Id));
        commandBus.Dispatch(new ChangePersonalInformationFromUser(personId, "Ib", null, null, genders[2].Id));
        commandBus.Dispatch(new ChangePersonalInformationFromUser(personId, null, "Dib", null, genders[3].Id));

        //unitOfWork.Save(); // Given the in-memory nature of the mock contextes and the coding, there is no reason to call the save function.
    }
}
