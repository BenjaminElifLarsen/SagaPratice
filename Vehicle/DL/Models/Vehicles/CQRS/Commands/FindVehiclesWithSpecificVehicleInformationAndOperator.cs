using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class FindVehiclesWithSpecificVehicleInformationAndOperator : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public Guid OperatorId { get; private set; }
    public IEnumerable<Guid> VehicleInformationIds { get; private set; }

    public FindVehiclesWithSpecificVehicleInformationAndOperator(Guid operatorId, IEnumerable<Guid> vehicleInformationIds, Guid correlationId, Guid causationId)
    {
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
        OperatorId = operatorId;
        VehicleInformationIds = vehicleInformationIds;
    }
}
