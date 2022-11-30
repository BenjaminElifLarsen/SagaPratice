using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result> ChangePersonalInformationAsync(ChangePersonalInformationFromUser command)
    {
        return await Task.Run(() => _commandBus.Send(command));
    }
}
