using PeopleDomain.DL.CQRS.Queries.ReadModels;

namespace PeopleDomain.DL.Validation;
internal class GenderValidationData
{
    public IEnumerable<GenderVerb> GenderVerbs { get; private set; }

	public GenderValidationData(IEnumerable<GenderVerb> genderVerbs)
	{
		GenderVerbs = genderVerbs;		
	}
}
