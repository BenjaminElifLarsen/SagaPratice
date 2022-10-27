using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Validation;

namespace VehicleDomain.DL.Models.Operators;
internal interface IOperatorFactory
{
    public Result<Operator> CreateOperator(AddOperatorNoLicenseFromSystem @operator);
    public Result<Operator> CreateOperator(AddOperatorWithLicenseFromUser @operator, OperatorValidationData validationData, PersonCreationLicenseValidationData licenseValidationData);
}