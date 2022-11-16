using Common.CQRS.Commands;
using PeopleDomain.AL.Handlers.Command;

namespace PeopleDomain.AL.Busses.Command;
internal class MockCommandBus : ICommandBus
{
    private readonly IPeopleCommandHandler _commandHandler;
    public MockCommandBus(IPeopleCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public void Publish<T>(T command) where T : ICommand
    {
        throw new NotImplementedException();
    }

    public void RegisterHandler<T>(Action<T> handler) where T : ICommand
    {
        throw new NotImplementedException();
    }

    public void UnregisterHandler<T>(Action<T> handler) where T : ICommand
    {
        throw new NotImplementedException();
    }
}
