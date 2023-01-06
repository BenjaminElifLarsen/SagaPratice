using Common.CQRS.Commands;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Handlers.Command;
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
