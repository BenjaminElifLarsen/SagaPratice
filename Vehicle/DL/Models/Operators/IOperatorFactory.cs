using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Validation;

namespace VehicleDomain.DL.Models.Operators;
internal interface IOperatorFactory
{
    public Result<Operator> CreateOperator(AddPersonNoLicenseFromSystem person);
    public Result<Operator> CreateOperator(AddPersonWithLicenseFromUser person, OperatorValidationData validationData, PersonCreationLicenseValidationData licenseValidationData);
}