using Common.CQRS.Commands;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.CQRS.Commands.Handlers;
public interface IPeopleCommandHandler :
    ICommandHandler<HirePersonFromUser>,
    ICommandHandler<FirePersonFromUser>,
    ICommandHandler<ChangePersonalInformationFromUser>,
    ICommandHandler<RecogniseGender>
{
}
