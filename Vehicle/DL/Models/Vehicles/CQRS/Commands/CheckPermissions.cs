using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class CheckPermissions : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public Guid OperatorId { get; private set; }
    
    public Guid VehicleId { get; private set; }

    internal CheckPermissions(Guid operatorId, Guid vehicleId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
