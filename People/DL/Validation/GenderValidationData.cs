using PeopleDomain.DL.CQRS.Queries.ReadModels;

namespace PeopleDomain.DL.Validation;
internal class GenderValidationData
{
    public IEnumerable<GenderVerbValidation> GenderVerbs { get; private set; }

	public GenderValidationData(IEnumerable<GenderVerbValidation> genderVerbs)
	{
		GenderVerbs = genderVerbs;		
	}
}
