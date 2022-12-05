using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal class AlterLicenseTypeProcessManager : IAlterLicenseTypeProcessManager
{ //when altering a license type 2 important events are created
  //the question is if this process manager should handle both or only a single one
  //does it even make sense to have multiple process managers?
  //would it be better to have an event that states what have changed, which is sent to the process manager?
    private readonly IVehicleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public AlterLicenseTypeProcessManager(IVehicleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void Handler(LicenseTypeAlteredSuccesed @event)
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
        throw new NotImplementedException();
    }

    public void SetCallback(Action<Result> callback)
    {
        throw new NotImplementedException();
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
