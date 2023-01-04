using Common.BinaryFlags;
using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.LicenseTypes.Validation;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.VehicleInformations.Validation;
using VehicleDomain.DL.Models.Vehicles;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries;
using VehicleDomain.DL.Models.Vehicles.Events;
using VehicleDomain.DL.Models.Vehicles.Validation;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Handlers.Command;
internal sealed class VehicleCommandHandler : IVehicleCommandHandler
{
    private readonly IVehicleFactory _vehicleFactory;
    private readonly IOperatorFactory _operatorFactory;
    private readonly ILicenseTypeFactory _licenseTypeFactory;
    private readonly IVehicleInformationFactory _vehicleInformationFactory;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleCommandHandler(IOperatorFactory operatorFactory,
        ILicenseTypeFactory licenseTypeFactory,
        IVehicleInformationFactory vehicleInformationFactory,
        IVehicleFactory vehicleFactory,
        IUnitOfWork unitOfWork)
    {
        _vehicleFactory = vehicleFactory;
        _operatorFactory = operatorFactory;
        _licenseTypeFactory = licenseTypeFactory;
        _vehicleInformationFactory = vehicleInformationFactory;
        _unitOfWork = unitOfWork;
    }

    public void Handle(ValidateOperatorLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            throw new NotImplementedException();
            return; // new InvalidResultNoData("Operator was not found.");
        }
        var license = @operator.GetLicenseViaLicenseType(command.TypeId);
        if (license is null)
        {

            throw new NotImplementedException();
            //return; // new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        throw new NotImplementedException();
        var oldValue = license.Expired; //consider rewritting this section
        license.CheckIfExpired();
        if (oldValue != license.Expired)
        {
            _unitOfWork.OperatorRepository.Update(@operator);
            _unitOfWork.Save();
        }
    }

    public void Handle(AddOperatorNoLicenseFromSystem command)
    {
        if (!_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return; // new InvalidResultNoData("Operator already exist.");
        }
        var result = _operatorFactory.CreateOperator(command);
        if (result is InvalidResult<Operator>)
        {
            return; // new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.OperatorRepository.Create(result.Data);
        try
        {
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return; // new InvalidResultNoData(e.Message);
        }
        return; // new SuccessResultNoData();
    }

    public void Handle(AddLicenseToOperator command)
    {
        if (_unitOfWork.LicenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return; // new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            return; // new InvalidResultNoData("Operator was not found.");
        }
        if (@operator.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return; // new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _unitOfWork.OperatorRepository.Update(@operator);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(EstablishLicenseTypeFromUser command)
    {
        if (!_unitOfWork.LicenseTypeRepository.IsTypeUniqueAsync(command.Type).Result)
        {
            return; // new InvalidResultNoData($"License type with of id {command.Type} already exist.");
        }
        var result = _licenseTypeFactory.CreateLicenseType(command); //consider moving all this stuff into a domain model (or multiple domain models)
        if (result is InvalidResult<LicenseType>)
        {
            return; // new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.LicenseTypeRepository.Create(result.Data);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(RemoveOperatorFromSystem command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete();
            entity.AddDomainEvent(new OperatorRemoved(entity, command.CorrelationId, command.CommandId));
            _unitOfWork.OperatorRepository.Update(entity);
            _unitOfWork.Save();
        }
        return; // new SuccessResultNoData();
    }


    public void Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event, ObsoletedLicenseType, that trigger expiring driving licenses that people may have that use this specific type id.
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.MomentOfDeletion.Year, command.MomentOfDeletion.Month, command.MomentOfDeletion.Day));
            _unitOfWork.LicenseTypeRepository.Update(entity);
            _unitOfWork.Save();
        }
        return; // new SuccessResultNoData();
    }

    public void Handle(AlterLicenseType command)
    { //not fully updated to use events, need failer event and not found event
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if(entity is null)
        {
            _unitOfWork.AddSystemEvent(new LicenseTypeAlteredFailed(command.Id, new string[] {"Not found."}, command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); 
            return; // new InvalidResultNoData("Not found.");
        }

        var flag = new LicenseTypeChangeInformationValidator(command).Validate();
        if (!flag)
        {
            var errors = LicenseTypeErrorConversion.Convert(flag);
            _unitOfWork.AddSystemEvent(new LicenseTypeAlteredFailed(entity,errors, command.CorrelationId, command.CommandId));
            _unitOfWork.LicenseTypeRepository.Update(entity);
            _unitOfWork.Save();
            return; // new InvalidResultNoData(errors.ToArray());
        }
        bool ageChanged = false;
        bool renewChanged = false;
        bool typeChanged = false;
        if(command.Type is not null) //should it be possible to change the type if the license type is in use? Could validate against it by having in the ctor a operator/vehicle amount and a specific specification for it.
        {
            entity.ReplaceType(command.Type.Type);
        }
        if(command.AgeRequirement is not null)
        { //trigger event
            ageChanged = true;
            entity.ChangeAgeRequirement(command.AgeRequirement.AgeRequirement);
            entity.AddDomainEvent(new LicenseTypeAgeRequirementChanged(entity, command.CorrelationId, command.CommandId));
        }
        if(command.RenewPeriod is not null)
        { //trigger event
            renewChanged = true;
            entity.ChangeRenewPeriod(command.RenewPeriod.RenewPeriod);
            entity.AddDomainEvent(new LicenseTypeRenewPeriodChanged(entity, command.CorrelationId, command.CommandId));
        }
        entity.AddDomainEvent(new LicenseTypeAlteredSucceeded(entity, typeChanged, ageChanged, renewChanged, command.CorrelationId, command.CommandId));
        _unitOfWork.LicenseTypeRepository.Update(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(AddVehicleInformationFromSystem command)
    {
        if (!_unitOfWork.VehicleInformationRepository.IsNameUniqueAsync(command.VehicleName).Result)
        {
            return; // new InvalidResultNoData("Vehicle information is not unique.");
        }
        var data = _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeForVehicleInformationValidationQuery()).Result;
        var valdationData = new VehicleInformationValidationData(data);
        var result = _vehicleInformationFactory.CreateVehicleInformation(command, valdationData);
        if (result is InvalidResult<VehicleInformation>)
        {
            return; // new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleInformationRepository.Create(result.Data);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(BuyVehicleWithNoOperator command)
    { //trigger event VehicleAdded
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var validationData = new VehicleValidationData(vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, validationData);
        if (result is InvalidResult<Vehicle>)
        {
            return; // new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(BuyVehicleWithOperators command) //remove
    { //trigger event VehicleAdded.
        var operators = _unitOfWork.OperatorRepository.AllAsync(new OperatorIdQuery()).Result;
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var valiationData = new VehicleValidationWithOperatorsData(operators, vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, valiationData);
        if (result is InvalidResult<Vehicle>)
        {
            return; // new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(AddDistanceToVehicleDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData($"Not found.");
        }
        BinaryFlag flag = entity.AddToDistanceMoved(command.DistanceToAdd);
        if (!flag)
        {
            return; // new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(ResetVehicleMovedDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.OverwriteDistanceMoved(command.NewDistance);
        if (!flag)
        {
            return; // new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(EstablishRelationBetweenOperatorAndVehicle command)
    { //raise some event that generates commands for the two handlers below
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public void Handle(AddOperatorToVehicle command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData($"");
        }
        entity.AddOperator(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(AddVehicleToOperator command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData($"");
        }
        entity.AddVehicle(new(command.VehicleId));
        _unitOfWork.OperatorRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(RemoveRelationBetweenOperatorAndVehicle command)
    {
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return; // new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public void Handle(RemoveOperatorFromVehicle command)
    { 
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new VehicleNotFound(command.VehicleId, new string[] { "Not found." }, command.CorrelationId, command.CommandId)); ;
            return; // new InvalidResultNoData($"");
        }
        var removed = entity.RemoveOperator(new(command.OperatorId));
        if (removed)
        {
            entity.AddDomainEvent(new VehicleRemovedOperator(entity, command.OperatorId, command.CorrelationId, command.CommandId));
        }
        else
        {
            _unitOfWork.AddSystemEvent(new VehicleNotRequiredToRemoveOperator(entity, command.CorrelationId, command.CommandId));
        }

        _unitOfWork.VehicleRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(RemoveVehicleFromOperator command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null) //needs events
        {
            _unitOfWork.AddSystemEvent(new OperatorNotFound(command.OperatorId, new string[] { "Not found." }, command.CorrelationId, command.CommandId));
            return; // new InvalidResultNoData($"");
        }
        entity.RemoveVehicle(new(command.VehicleId));
        entity.AddDomainEvent(new OperatorRemovedVehicle(entity, command.VehicleId, command.CorrelationId, command.CommandId));
        _unitOfWork.OperatorRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(AttemptToStartVehicle command)
    {

        _unitOfWork.AddSystemEvent(new AttemptToStartVehicleStarted(command.VehicleId, command.OperatorId, command.CorrelationId, command.CommandId));
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
        //var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        //if (entity is null)
        //{
        //    return; // new InvalidResultNoData($"");
        //}
        //if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        //{
        //    return; // new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        //}
        //entity.StartOperating(new(command.OperatorId));
        //_unitOfWork.VehicleRepository.Update(entity);
        //_unitOfWork.Save();
        //return; // new SuccessResultNoData();
    }

    public void Handle(StopOperatingVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData($"Not found.");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return; // new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StopOperating(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(RemoveOperatorFromLicenseType command)
    {
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.LicenseTypeId).Result;
        if (entity is null)
        {
            //error most likely as this should only be called by an event
        }
        entity.RemoveOperator(new(command.OperatorId));
        entity.AddDomainEvent(new LicenseTypeOperatorRemoved(entity, command.OperatorId, command.CorrelationId, command.CommandId));
        _unitOfWork.LicenseTypeRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(ValidateLicenseAgeRequirementBecauseChange command)
    {
        foreach(var id in command.OperatorIds)
        {
            var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(id).Result;
            if(entity is null)
            { //trigger event that removes operator from license type
                continue;
            }

            //check if entity age is below the required age, if it is remove the specific license
            if(entity.CalculateAge() < command.AgeRequirement)
            { //implement age calculator
                //trigger event that removes the operator from license type and vehicle(s)
                var license = entity.GetLicenseViaLicenseType(command.LicenseTypeId);
                entity.RemoveLicense(license); //need to remove them for any vehicle they may have access too with the license
                //so more events. This might require changing how events are fetched from the context as it might not get new ones.
                //when run through all currently known events, could make another check for events and publish any known ones. Continue until there are no more events.
                //so some kind of recursive method (might not end up as a recursive method) that when event collection is empty make a check if there are new events. If there are run though them else return; // void.
                //this will also move the evnet publish code in UnitOfWork.Save() out of that method.
                //consider process manager for dealing with this.
                _unitOfWork.OperatorRepository.Update(entity);
                //the license type needs to know if a license is removed, so it can remove the operator
            }           
        }
        //tigger event LicenseTypeAgeRequirementValidatedSuccessed
        throw new NotImplementedException();
        return; // new SuccessResultNoData();
    }

    public void Handle(ValidateLicenseRenewPeriodBecauseChange command)
    {
        //need a method on license to renew. Either that or on operator.
        //currently CheckIfExpired might be possible to recycle for this purpose.
        //if operator is not found trigger event for remove operator from licnese type.
        //if operator license type cannot be removed trigger event for removing them for license type and vehicld
        //at the end trigger LicenseTypeRenewPeriodValidatedSucessed
        throw new NotImplementedException();
    }

    public void Handle(LicenseAgeRequirementRequireValidation command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if(entity is null)
        {
            _unitOfWork.AddSystemEvent(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return; // new InvalidResultNoData("Not Found.");
        }

        var license = entity.GetLicenseViaLicenseType(command.LicenseTypeId);
        if(license is null)
        {
            _unitOfWork.AddSystemEvent(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return; // new InvalidResultNoData("License Not Found.");
        }

        var status = entity.ValidateLicenseAgeRequirementIsFulfilled(command.NewAgeRequirement, command.LicenseTypeId);
        if (status == true)
        {
            entity.AddDomainEvent(new OperatorLicenseAgeRequirementValidated(entity, command.CorrelationId, command.CommandId)); ;
        }
        else
        {
            entity.RemoveLicense(license);
            entity.AddDomainEvent(new OperatorLicenseRetracted(entity, license, command.CorrelationId, command.CommandId)); ;
        }
        _unitOfWork.OperatorRepository.Update(entity);

        return; // new SuccessResultNoData();
    }

    public void Handle(LicenseRenewPeriodRequireValidation command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return; // new InvalidResultNoData("Not Found.");
        }

        var license = entity.GetLicenseViaLicenseType(command.LicenseTypeId);
        if (license is null)
        {
            _unitOfWork.AddSystemEvent(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return; // new InvalidResultNoData("License Not Found.");
        }

        var status = entity.ValidateLicenseRenewPeriodIsFulfilled(command.NewRenewPeriod, command.LicenseTypeId);
        if (status == true)
        {
            entity.AddDomainEvent(new OperatorLicenseRenewPeriodValidated(entity, command.CorrelationId, command.CommandId));
        }
        else
        {
            entity.AddDomainEvent(new OperatorLicenseExpired(entity, command.LicenseTypeId, command.CorrelationId, command.CommandId));
        }
        _unitOfWork.OperatorRepository.Update(entity);

        return; // new SuccessResultNoData();
    }

    public void Handle(FindVehicleInformationsWithSpecificLicenseType command)
    {
        var list = _unitOfWork.VehicleInformationRepository.FindAllWithSpecificLicenseTypeId(command.LicenseTypeId, new VehicleInformationIdQuery()).Result;
        _unitOfWork.AddSystemEvent(new FoundVehicleInformations(command.OperatorId, list.Select(x => x.Id), command.CorrelationId, command.CommandId));
        return; // new SuccessResultNoData();
    }

    public void Handle(FindVehiclesWithSpecificVehicleInformationAndOperator command)
    {
        var list = _unitOfWork.VehicleRepository.FindSpecificByOperatorIdAndVehicleInformationsAsync(command.OperatorId, command.VehicleInformationIds, new VehicleIdQuery()).Result;
        _unitOfWork.AddSystemEvent(new VehiclesFoundWithSpecificVehicleInformationAndOperator(command.OperatorId, list.Select(x => x.Id), command.CorrelationId, command.CommandId));
        return; // new SuccessResultNoData();
    }

    public void Handle(CheckPermissions command)
    {
        //have repo methods to check against the context, see Design.txt
        throw new NotImplementedException();
    }

    public void Handle(StartVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        //should handle a null reference, should not happen but may in a real enviroment
        if (entity.InUse)
        {
            _unitOfWork.AddSystemEvent(new VehicleStartedFailed(command.VehicleId, new string[] { "Already started." }, command.CorrelationId, command.CommandId));
            _unitOfWork.VehicleRepository.Update(entity);
            return; // new InvalidResultNoData();
        }
        entity.StartOperating(new(command.OperatorId)); //a vehicle that is in the process of being sold should not permit being operated?
        _unitOfWork.VehicleRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(FindOperator command)
    {
        var exist = !_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result;
        if (exist)
        {
            _unitOfWork.AddSystemEvent(new OperatorWasFound(command.OperatorId, command.CorrelationId, command.CommandId));
            return; // new SuccessResultNoData();
        }
        _unitOfWork.AddSystemEvent(new OperatorNotFound(command.OperatorId, new string[] { "Not found." }, command.CorrelationId, command.CommandId));
        return; // new InvalidResultNoData();
    }

    public void Handle(FindVehicle command)
    {
        var exist = _unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result;
        if (exist)
        {
            _unitOfWork.AddSystemEvent(new VehicleWasFound(command.VehicleId, command.CorrelationId, command.CommandId));
            return; // new SuccessResultNoData();
        }

        _unitOfWork.AddSystemEvent(new VehicleNotFound(command.VehicleId, new string[] { "Not found." }, command.CorrelationId, command.CommandId));
        return; // new InvalidResultNoData();
    }
}
