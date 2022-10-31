using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands.Handlers;
internal interface IPeopleCommandHandler : 
    ICommandHandler<HirePersonFromUser>,
    ICommandHandler<FirePersonFromUser>,
    ICommandHandler<ChangePersonalInformationFromUser>,
    ICommandHandler<RecogniseGender>
{
}
