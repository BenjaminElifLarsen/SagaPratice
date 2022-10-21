using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.People.Validation;

namespace VehicleDomain.DL.Models.People;
internal interface IPersonFactory
{
    public Result<Person> CreatePerson(AddPersonNoLicenseFromSystem person);
    public Result<Person> CreatePerson(AddPersonWithLicenseFromUser person, PersonValidationData validationData, PersonCreationLicenseValidationData licenseValidationData);
}


//internal class LicenseValidationData 
//{
//    public IEnumerable<LicenseType> LicenseTypes { get; private set; } //change type, figure out what is needed of data
//    public LicenseValidationData(IEnumerable<LicenseType> licenseTypes)
//    {
//        LicenseTypes = licenseTypes;
//    }
//}