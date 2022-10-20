using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes;

namespace VehicleDomain.DL.Models.People;
internal interface IPersonFactory
{
    public Result<Person> CreatePerson(AddPersonNoLicenseFromSystem person);
    public Result<Person> CreatePerson(AddPersonWithLicenseFromUser person, LicenseValidationData validationData);
}


internal class LicenseValidationData //used by a validator, either move it or crate a new model for the valdiator
{
    public IEnumerable<LicenseType> LicenseTypes { get; private set; } //change type
    public LicenseValidationData(IEnumerable<LicenseType> licenseTypes)
    {
        LicenseTypes = licenseTypes;
    }
}