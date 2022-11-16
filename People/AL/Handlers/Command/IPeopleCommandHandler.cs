using Common.CQRS.Commands;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Command;
public interface IPeopleCommandHandler :
    ICommandHandler<HirePersonFromUser>,
    ICommandHandler<FirePersonFromUser>,
    ICommandHandler<ChangePersonalInformationFromUser>,
    ICommandHandler<RecogniseGender>,
    ICommandHandler<UnrecogniseGender>,
    ICommandHandler<AddPersonToGender, PersonHired>,
    ICommandHandler<RemovePersonFromGender, PersonFired>,
    ICommandHandler<ChangePersonGender, PersonChangedGender>
{
}
