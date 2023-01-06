using Common.CQRS.Commands;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Handlers.Command;
public interface IPersonCommandHandler :
    ICommandHandler<HirePersonFromUser>,
    ICommandHandler<FirePersonFromUser>,
    ICommandHandler<ChangePersonalInformationFromUser>,
    ICommandHandler<RecogniseGender>,
    ICommandHandler<UnrecogniseGender>,
    ICommandHandler<AddPersonToGender>,
    ICommandHandler<RemovePersonFromGender>,
    ICommandHandler<ChangePersonGender>
{
}
