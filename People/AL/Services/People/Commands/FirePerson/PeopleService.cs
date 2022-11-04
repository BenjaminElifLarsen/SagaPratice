using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result> FirePersonAsync(FirePersonFromUser command)
    {
        return await Task.Run(() => _peopleCommandHandler.Handle(command));
    }
}
