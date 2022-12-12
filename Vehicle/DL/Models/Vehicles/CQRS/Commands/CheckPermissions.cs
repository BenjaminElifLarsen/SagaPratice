using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
internal class CheckPermissions : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int OperatorId { get; private set; }
    
    public int VehicleId { get; private set; }

    public CheckPermissions(int operatorId, int vehicleId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
