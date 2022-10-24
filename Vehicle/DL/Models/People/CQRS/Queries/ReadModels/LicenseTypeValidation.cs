using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;
internal class LicenseTypeAgeValidation : BaseReadModel
{
	public int Id { get; private set; }
    public byte YearRequirement { get; private set; }

	public LicenseTypeAgeValidation(byte yearRequirement, int id)
	{
		YearRequirement = yearRequirement;
		Id = id;
	}
}
