using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries.ReadModels;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Validation.PersonSpecifications;
internal sealed class IsPersonGenderValid : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
{ //consider a better name

    private IEnumerable<GenderIdValidation> _genderIds;
    public IsPersonGenderValid(PersonValidationData validationData)
    {
        _genderIds = validationData.GenderIds;
    }

    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Gender);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(ChangePersonalInformationFromUser candidate)
    {
        return candidate.Gender is null || IsSatisfiedBy(candidate.Gender.Gender);
    }

    private bool IsSatisfiedBy(Guid candidate)
    {
        return _genderIds.Any(x => x.Id == candidate);
    }
}
