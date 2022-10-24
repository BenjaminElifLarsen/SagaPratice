using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Models.People.Validation;
using LicenseValidationData = VehicleDomain.DL.Models.People.Validation.PersonCreationLicenseValidationData.LicenseValidationData;

namespace VehicleDomain.DL.Models.People;
internal class PersonFactory : IPersonFactory
{ 
    public Result<Person> CreatePerson(AddPersonNoLicenseFromSystem person)
    {
        List<string> errors = new(); //need to check if id is in use, maybe do that outside the factory

        int flag = new PersonValidatorFromSystem(person).Validate();
        if(flag != 0)
        {
            errors.AddRange(PersonErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Person>(errors.ToArray());
        }

        Person entity = new(person.Id, person.Birth);
        return new SuccessResult<Person>(entity);
    }

    public Result<Person> CreatePerson(AddPersonWithLicenseFromUser person, PersonValidationData validationData, PersonCreationLicenseValidationData licenseValidationData)
    {
        List<string> errors = new();

        //ensure there is not licenseTypeId duplicates 
        bool licenseErrorFound = false;
        foreach(var licenseTypeId in person.Licenses)
        {
            LicenseValidationData valdation = licenseValidationData.LicenseTypes[licenseTypeId.LicenseTypeId];
            int lFlag = new PersonLicenseValidatorFromUser(licenseTypeId, valdation, licenseValidationData.PermittedLicenseTypeIds).Validate();
            if(lFlag != 0)
            {
                if(licenseErrorFound == false)
                {
                    licenseErrorFound = true;
                }
                errors.AddRange(LicenseErrorConversion.Convert(lFlag, licenseTypeId));
            }
        }

        int flag = new PersonValidatorFromUser(person, validationData, 80, 10).Validate();
        if(flag != 0)
        {
            errors.AddRange(PersonErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Person>(errors.ToArray());
        }

        Person entity = new(person.Id, person.Birth);
        foreach(var licenseTypeId in person.Licenses)
        {
            entity.AddLicense(new(licenseTypeId.LicenseTypeId), licenseTypeId.Arquired);
        }

        return new SuccessResult<Person>(entity);
    }
}
