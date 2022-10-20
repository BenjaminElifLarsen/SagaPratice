using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;
internal class LicenseTypeAgeValidation : BaseReadModel
{
    public byte YearRequirement { get; private set; }

	public LicenseTypeAgeValidation(byte yearRequirement)
	{
		YearRequirement = yearRequirement;
	}
}
