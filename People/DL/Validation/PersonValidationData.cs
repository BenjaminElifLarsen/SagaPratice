using PeopleDomain.DL.CQRS.Queries.ReadModels;

namespace PeopleDomain.DL.Validation;
internal class PersonValidationData
{
    public IEnumerable<GenderIdValidation> GenderIds { get; private set; }

	public PersonValidationData(IEnumerable<GenderIdValidation> genderIds)
	{
		GenderIds = genderIds;
	}
}
