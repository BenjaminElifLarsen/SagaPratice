using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class ObsoleteLicenseTypeFromUser : ICommand
{ //need to soft delete and expire any license that use it
    public int Id { get; private set; }
    public DateTime MomentOfDeletion { get; private set; }
}
