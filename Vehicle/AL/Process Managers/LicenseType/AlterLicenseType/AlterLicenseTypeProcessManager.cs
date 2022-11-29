using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
internal class AlterLicenseTypeProcessManager : IAlterLicenseTypeProcessManager
{ //when altering a license type 2 important events are created
  //the question is if this process manager should handle both or only a single one
  //does it even make sense to have multiple process managers?
  //would it be better to have an event that states what have changed, which is sent to the process manager?
    private readonly IVehicleCommandBus commandBus;
    
    public Guid ProcessManagerId { get; private set; }

    public AlterLicenseTypeProcessManager()
    {
        ProcessManagerId = Guid.NewGuid();
    }

    public void Handler(LicenseTypeAltered @event)
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
}
