using Common.Events.Bus;
using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Gender.Recognise;
public interface IRecogniseProcessManager : IBaseProcessManager, //have an concrete basic implementation of contract that can be used for inheritance context storage
    IEventHandler<GenderRecognisedSucceeded>, //to ensure all process managers can be stored in a single table, thus this class shall inherit from that new class
    IEventHandler<GenderRecognisedFailed>
{

}