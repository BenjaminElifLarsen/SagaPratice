using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.Validation;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries;
using VehicleDomain.DL.Models.VehicleInformations.Validation;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal class VehicleCommandHandler : IVehicleCommandHandler
{
    private readonly IOperatorFactory _personFactory;
    private readonly IOperatorRepository _personRepository;

    private readonly ILicenseTypeFactory _licenseTypeFactory;
    private readonly ILicenseTypeRepository _licenseTypeRepository;

    private readonly IVehicleInformationFactory _vehicleInformationFactory;
    private readonly IVehicleInformationRepository _vehicleInformationRepository;

    public VehicleCommandHandler(IOperatorFactory personFactory, IOperatorRepository personRepository, ILicenseTypeFactory licenseTypeFactory, ILicenseTypeRepository licenseTypeRepository, IVehicleInformationFactory vehicleInformationFactory, IVehicleInformationRepository vehicleInformationRepository)
    {
        _personFactory = personFactory;
        _personRepository = personRepository;
        _licenseTypeFactory = licenseTypeFactory;
        _licenseTypeRepository = licenseTypeRepository;
        _vehicleInformationFactory = vehicleInformationFactory;
        _vehicleInformationRepository = vehicleInformationRepository;
    }


    public Result Handle(ValidateDriverLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var @operator = _personRepository.GetForOperationAsync(command.OperatorId).Result;
        if(@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        var license = @operator.GetLicense(command.TypeId);
        if(license is null)
        {
            return new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        var oldValue = license.Expired;
        license.CheckIfExpired();
        if(oldValue != license.Expired)
        {
            _personRepository.Update(@operator);
            _personRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddPersonNoLicenseFromSystem command)
    { //will need to trigger an event, PersonCreated
        if (!_personRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var result = _personFactory.CreateOperator(command);
        if(result is InvalidResult<Operator>)
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
            return new InvalidResultNoData("Operator already exist.");
        }
        var licenseTypeAges = _licenseTypeRepository.AllAsync(new LicenseTypeAgeQuery()).Result;
        var licenseTypeIds = _licenseTypeRepository.AllAsync(new LicenseTypeIdQuery()).Result;
        OperatorValidationData data = new(licenseTypeAges);
        var dictionary = new Dictionary<int, PersonCreationLicenseValidationData.LicenseValidationData>();
        foreach(var licenseType in licenseTypeAges)
        {
            dictionary.Add(licenseType.Id, new PersonCreationLicenseValidationData.LicenseValidationData(licenseType));
        }
        PersonCreationLicenseValidationData licenseData = new(dictionary, licenseTypeIds);
        var result = _personFactory.CreateOperator(command, data, licenseData);
        if (result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _personRepository.Create(result.Data);
        _personRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddLicenseToOperator command)
    {
        if (_licenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var @operator = _personRepository.GetForOperationAsync(command.PersonId).Result;
        if(@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        if(@operator.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _personRepository.Update(@operator);
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

    public Result Handle(RemoveOperatorFromSystem command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(RemoveOperatorFromUser command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event, ObsoletedLicenseType, that trigger expiring driving licenses that people may have that use this specific type id.
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

    public Result Handle(AddVehicleInformationFromExternalSystem command)
    {
        if (!_vehicleInformationRepository.IsNameUniqueAsync(command.VehicleName).Result)
        {
            return new InvalidResultNoData("Vehicle information is not unique.");
        }
        var data = _licenseTypeRepository.AllAsync(new LicenseTypeForVehicleInformationValidationQuery()).Result;
        var valdationData = new VehicleInformationValidationData(data);
        var result = _vehicleInformationFactory.CreateVehicleInformation(command, valdationData);
        if(result is InvalidResult<VehicleInformation>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _vehicleInformationRepository.Create(result.Data);
        _vehicleInformationRepository.Save();
        return new SuccessResultNoData();
    }
}
