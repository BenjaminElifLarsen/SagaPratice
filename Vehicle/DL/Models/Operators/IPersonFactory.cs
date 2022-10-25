using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Validation;

namespace VehicleDomain.DL.Models.Operators;
internal interface IPersonFactory
{
    public Result<Operator> CreatePerson(AddPersonNoLicenseFromSystem person);
    public Result<Operator> CreatePerson(AddPersonWithLicenseFromUser person, OperatorValidationData validationData, PersonCreationLicenseValidationData licenseValidationData);
}


//internal class LicenseValidationData 
//{
//    public IEnumerable<LicenseType> LicenseTypes { get; private set; } //change type, figure out what is needed of data
//    public LicenseValidationData(IEnumerable<LicenseType> licenseTypes)
//    {
//        LicenseTypes = licenseTypes;
//    }
//}