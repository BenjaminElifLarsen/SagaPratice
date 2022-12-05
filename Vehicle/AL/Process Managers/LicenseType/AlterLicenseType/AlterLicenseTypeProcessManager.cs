using Common.ProcessManager;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.Events;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal class AlterLicenseTypeProcessManager : IAlterLicenseTypeProcessManager
{ 
    private readonly IVehicleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed; //how does this and the one below handle an empty dictionary? Pretty sure on the answer, but check to be sure

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public AlterLicenseTypeProcessManager(IVehicleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void Handler(LicenseTypeAlteredSuccessed @event)
    { //need to ensure that it cannot return the final event, that it is finished, until all 'paths' are done processing
        //could have enums, "not started", "waiting on", and "done" and then have a variable for each command that is going to be pushed. It goes from not started to waiting and can only go to done when an event is returned
        //if (true) //age requirement changed
        //{

        //}
        //var c1 = new ValidateLicenseAgeRequirementBecauseChange();
        //if (true) //renew period changed
        //{

        //}
        //var c2 = new ValidateLicenseRenewPeriodBecauseChange();

        throw new NotImplementedException();
    }

    public void SetUp(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public void PublishEventIfPossible()
    {
        throw new NotImplementedException();
    }

    public void RegistrateHandler(Action<ProcesserFinished> handler)
    {
        throw new NotImplementedException();
    }

}
