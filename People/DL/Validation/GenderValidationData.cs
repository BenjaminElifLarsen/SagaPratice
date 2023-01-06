using PersonDomain.DL.CQRS.Queries.ReadModels;

namespace PersonDomain.DL.Validation;
internal sealed class GenderValidationData
{
    public IEnumerable<GenderVerbValidation> GenderVerbs { get; private set; }

	public GenderValidationData(IEnumerable<GenderVerbValidation> genderVerbs)
	{
		GenderVerbs = genderVerbs;		
	}
}
