using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.People;
using VehicleDomain.DL.Models.People.CQRS.Queries;
using VehicleDomain.DL.Models.People.Validation;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal class VehicleCommandHandler : IVehicleCommandHandler
{
    private readonly IPersonRepository _personRepository;
    private readonly IPersonFactory _personFactory;

    private readonly ILicenseTypeFactory _licenseTypeFactory;
    private readonly ILicenseTypeRepository _licenseTypeRepository;

    public VehicleCommandHandler(IPersonFactory personFactory, IPersonRepository personRepository, ILicenseTypeFactory licenseTypeFactory, ILicenseTypeRepository licenseTypeRepository)
    {
        _personFactory = personFactory;
        _personRepository = personRepository;
        _licenseTypeFactory = licenseTypeFactory;
        _licenseTypeRepository = licenseTypeRepository;
    }


    public Result Handle(ValidateDriverLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var person = _personRepository.GetForOperationAsync(command.OwnerId).Result;
        if(person is null)
        {
            return new InvalidResultNoData("Person was not found.");
        }
        var license = person.GetLicense(command.TypeId);
        if(license is null)
        {
            return new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        var oldValue = license.Expired;
        license.CheckIfExpired();
        if(oldValue != license.Expired)
        {
            _personRepository.Update(person);
            _personRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddPersonNoLicenseFromSystem command)
    { //will need to trigger an event, PersonCreated
        if (!_personRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Person already exist.");
        }
        var result = _personFactory.CreatePerson(command);
        if(result is InvalidResult<Person>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _personRepository.Create(result.Data);
        _personRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddPersonWithLicenseFromUser command)
    {//will need to trigger an event, PersonCreated
        if (!_personRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Person already exist.");
        }
        var licenseTypeAges = _licenseTypeRepository.AllAsync(new LicenseTypeAgeQuery()).Result;
        var licenseTypeIds = _licenseTypeRepository.AllAsync(new LicenseTypeIdQuery()).Result;
        PersonValidationData data = new(licenseTypeAges);
        var dictionary = new Dictionary<int, PersonCreationLicenseValidationData.LicenseValidationData>();
        foreach(var licenseType in licenseTypeAges)
        {
            dictionary.Add(licenseType.Id, new PersonCreationLicenseValidationData.LicenseValidationData(licenseType));
        }
        PersonCreationLicenseValidationData licenseData = new(dictionary, licenseTypeIds);
        var result = _personFactory.CreatePerson(command, data, licenseData);
        if (result is InvalidResult<Person>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _personRepository.Create(result.Data);
        _personRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddLicenseToPerson command)
    {
        if (_licenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var person = _personRepository.GetForOperationAsync(command.PersonId).Result;
        if(person is null)
        {
            return new InvalidResultNoData("Person was not found.");
        }
        if(person.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _personRepository.Update(person);
        _personRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishLicenseTypeFromUser command)
    {
        if (!_licenseTypeRepository.IsTypeUniqueAsync(command.Type).Result)
        {
            return new InvalidResultNoData($"License type with of id {command.Type} already exist.");
        }
        var result = _licenseTypeFactory.CreateLicenseType(command);
        if(result is InvalidResult<LicenseType>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _licenseTypeRepository.Create(result.Data);
        _licenseTypeRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemovePersonFromSystem command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(RemovePersonFromUser command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event that trigger expiring driving licenses that people may have that use this specific type id.
        var entity = _licenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if(entity is not null)
        {
            entity.Delete(command.MomentOfDeletion);
            _licenseTypeRepository.Update(entity);
            _licenseTypeRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AlterLicenseType command)
    {
        throw new NotImplementedException();
    }
}
