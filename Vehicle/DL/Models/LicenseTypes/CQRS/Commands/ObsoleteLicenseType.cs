using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class ObsoleteLicenseTypeFromUser : ICommand
{ //need to soft delete and expire any license that use it
    public int Id { get; set; }
    public DateTime MomentOfDeletion { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ObsoleteLicenseTypeFromUser()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    public ObsoleteLicenseTypeFromUser(int id, DateTime momenetOfDeletion, Guid correlationId, Guid causationId)
    {
        Id = id;
        MomentOfDeletion = momenetOfDeletion;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
