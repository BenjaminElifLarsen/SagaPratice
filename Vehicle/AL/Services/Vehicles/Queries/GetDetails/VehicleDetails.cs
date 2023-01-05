using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
public record VehicleDetails : BaseReadModel
{
    public Guid Id { get; private set; }
    public DateTime ProductionDate { get; private set; }
    public double DistancedMoved { get; private set; }
    public bool IsCurrentlyBeingOperated { get; private set; }
    public string SerielNumber { get; private set; }
    public IEnumerable<Guid> OperatorIds { get; private set; }

    public VehicleDetails(Guid id, DateTime productionDate, double distancedMoved, bool isCurrentlyBeingOperated, string serielNumber, IEnumerable<Guid> operatorIds)
    {
        Id = id;
        ProductionDate = productionDate;
        DistancedMoved = distancedMoved;
        IsCurrentlyBeingOperated = isCurrentlyBeingOperated;
        SerielNumber = serielNumber;
        OperatorIds = operatorIds;
    }
}
