using Common.ResultPattern;

namespace PeopleDomain.DL.CQRS.Commands.Handlers;
internal class PeopleCommandHandler : IPeopleCommandHandler
{
    public Result Handle(HirePersonFromUser command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(FirePersonFromUser command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    {
        throw new NotImplementedException();
    }
}
