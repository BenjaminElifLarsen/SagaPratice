using Common.CQRS.Queries;

namespace VehicleDomain.AL.Services.LicenseTypes.Queries.GetList;
public record LicenseTypeListItem : BaseReadModel
{
	public Guid Id { get; private set; }
	public string Type { get; private set; }
	public byte RenewPeriod { get; private set; }
	public byte AgeRequirement { get; private set; }
	public int AmountOfOperators { get; private set; }
	public int AmountOfVehicleInformatons { get; private set; }

	public LicenseTypeListItem(Guid id, string type, byte renewPeriod, byte ageRequirement, int operatorAmount, int vehicleInformationAmount)
	{
		Id = id;
		Type = type;
		RenewPeriod = renewPeriod;
		AgeRequirement = ageRequirement;
		AmountOfOperators = operatorAmount;
		AmountOfVehicleInformatons = vehicleInformationAmount;
	}
}
