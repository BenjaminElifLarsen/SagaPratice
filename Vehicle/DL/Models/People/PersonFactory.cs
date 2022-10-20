using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.People;
internal class PersonFactory : IPersonFactory
{ //can a person at creation have a license?
    public Result<Person> CreatePerson(AddPersonNoLicenseFromSystem person)
    {
        throw new NotImplementedException();
    }

    public Result<Person> CreatePerson(AddPersonWithLicenseFromUser person, LicenseValidationData validationData)
    {
        throw new NotImplementedException();
    }
}
