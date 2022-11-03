using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
public class VehicleDetails : BaseReadModel
{
    public int Id { get; private set; }
    public DateTime ProductionDate { get; private set; }
    public double DistancedMoved { get; private set; }
    public bool IsCurrentlyBeingOperated { get; private set; }
    public string SerielNumber { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public VehicleDetails(int id, DateTime productionDate, double distancedMoved, bool isCurrentlyBeingOperated, string serielNumber, IEnumerable<int> operatorIds)
    {
        Id = id;
        ProductionDate = productionDate;
        DistancedMoved = distancedMoved;
        IsCurrentlyBeingOperated = isCurrentlyBeingOperated;
        SerielNumber = serielNumber;
        OperatorIds = operatorIds;
    }
}
