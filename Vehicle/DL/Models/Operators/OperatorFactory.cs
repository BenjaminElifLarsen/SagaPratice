using Common.Other;
using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Validation;
using LicenseValidationData = VehicleDomain.DL.Models.Operators.Validation.PersonCreationLicenseValidationData.LicenseValidationData;

namespace VehicleDomain.DL.Models.Operators;
internal class OperatorFactory : IOperatorFactory
{
    public Result<Operator> CreateOperator(AddOperatorNoLicenseFromSystem @operator)
    {
        List<string> errors = new(); //need to check if id is in use, maybe do that outside the factory

        BinaryFlag flag = new OperatorValidatorFromSystem(@operator).Validate();
        if (flag != 0)
        {
            errors.AddRange(OperatorErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Operator>(errors.ToArray());
        }

        Operator entity = new(@operator.Id, new(@operator.Birth.Year, @operator.Birth.Month, @operator.Birth.Day));
        return new SuccessResult<Operator>(entity);
    }

    public Result<Operator> CreateOperator(AddOperatorWithLicenseFromUser @operator, OperatorValidationData validationData, PersonCreationLicenseValidationData licenseValidationData)
    {
        List<string> errors = new();

        //ensure there is not licenseTypeId duplicates 

        bool licenseErrorFound = false;
        foreach (var licenseTypeId in @operator.Licenses)
        {
            LicenseValidationData valdation = licenseValidationData.LicenseTypes[licenseTypeId.LicenseTypeId];
            BinaryFlag lFlag = new OperatorLicenseValidatorFromUser(licenseTypeId, valdation, licenseValidationData.PermittedLicenseTypeIds).Validate();
            if (lFlag != 0)
            {
                if (licenseErrorFound == false)
                {
                    licenseErrorFound = true;
                }
                errors.AddRange(LicenseErrorConversion.Convert(lFlag, licenseTypeId));
            }
        }

        BinaryFlag flag = new OperatorValidatorFromUser(@operator, validationData, 80, 10).Validate();
        if (flag != 0)
        {
            errors.AddRange(OperatorErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Operator>(errors.ToArray());
        }

        Operator entity = new(@operator.Id, new(@operator.Birth.Year, @operator.Birth.Month, @operator.Birth.Day));
        foreach (var licenseTypeId in @operator.Licenses)
        {
            entity.AddLicense(new(licenseTypeId.LicenseTypeId), new(licenseTypeId.Arquired.Year, licenseTypeId.Arquired.Month, licenseTypeId.Arquired.Day));
        }

        return new SuccessResult<Operator>(entity);
    }
}
