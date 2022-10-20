using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.People;
internal interface IPersonFactory
{
    public Result<Person> CreatePerson(AddPersonNoLicenseFromSystem person);
    public Result<Person> CreatePerson(AddPersonWithLicenseFromUser person, LicenseValidationData validationData);
}


internal class LicenseValidationData
{
    public IEnumerable<int> LicenseTypes { get; private set; }
    public LicenseValidationData(IEnumerable<int> licenseTypes)
    {
        LicenseTypes = licenseTypes;
    }
}