using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands.Handlers;
public interface IPeopleCommandHandler : 
    ICommandHandler<HirePersonFromUser>,
    ICommandHandler<FirePersonFromUser>,
    ICommandHandler<ChangePersonalInformationFromUser>,
    ICommandHandler<RecogniseGender>
{
} //maybe move this and its implementation into AL
