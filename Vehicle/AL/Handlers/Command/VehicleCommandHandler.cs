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
internal class VehicleCommandHandler : IVehicleCommandHandler
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

    public Result Handle(ValidateOperatorLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        var license = @operator.GetLicenseViaLicenseType(command.TypeId);
        if (license is null)
        {
            return new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        return new SuccessResultNoData();
        var oldValue = license.Expired; //consider rewritting this section
        license.CheckIfExpired();
        if (oldValue != license.Expired)
        {
            _unitOfWork.OperatorRepository.Update(@operator);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddOperatorNoLicenseFromSystem command)
    {
        if (!_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var result = _operatorFactory.CreateOperator(command);
        if (result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.OperatorRepository.Create(result.Data);
        try
        {
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddLicenseToOperator command)
    {
        if (_unitOfWork.LicenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        if (@operator.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _unitOfWork.OperatorRepository.Update(@operator);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishLicenseTypeFromUser command)
    {
        if (!_unitOfWork.LicenseTypeRepository.IsTypeUniqueAsync(command.Type).Result)
        {
            return new InvalidResultNoData($"License type with of id {command.Type} already exist.");
        }
        var result = _licenseTypeFactory.CreateLicenseType(command); //consider moving all this stuff into a domain model (or multiple domain models)
        if (result is InvalidResult<LicenseType>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.LicenseTypeRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromSystem command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete();
            entity.AddDomainEvent(new OperatorRemoved(entity, entity.Events.Count(), command.CorrelationId, command.CommandId));
            _unitOfWork.OperatorRepository.Update(entity);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }


    public Result Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event, ObsoletedLicenseType, that trigger expiring driving licenses that people may have that use this specific type id.
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.MomentOfDeletion.Year, command.MomentOfDeletion.Month, command.MomentOfDeletion.Day));
            _unitOfWork.LicenseTypeRepository.Update(entity);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AlterLicenseType command)
    {
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if(entity is null)
        {
            return new InvalidResultNoData("Not found.");
        }

        var flag = new LicenseTypeChangeInformationValidator(command).Validate();
        if (!flag)
        {
            return new InvalidResultNoData(LicenseTypeErrorConversion.Convert(flag).ToArray());
        }
        
        if(command.Type is not null) //should it be possible to change the type if the license type is in use? Could validate against it by having in the ctor a operator/vehicle amount and a specific specification for it.
        {
            entity.ReplaceType(command.Type.Type);
        }
        if(command.AgeRequirement is not null)
        { //trigger event
            entity.AddDomainEvent(new LicenseTypeAgeRequirementChanged(entity, entity.Events.Count(), command.CorrelationId, command.CommandId));
            entity.ChangeAgeRequirement(command.AgeRequirement.AgeRequirement);
        }
        if(command.RenewPeriod is not null)
        { //trigger event
            entity.AddDomainEvent(new LicenseTypeRenewPeriodChanged(entity, entity.Events.Count(), command.CorrelationId, command.CommandId));
            entity.ChangeRenewPeriod(command.RenewPeriod.RenewPeriod);
        }

        _unitOfWork.LicenseTypeRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddVehicleInformationFromSystem command)
    {
        if (!_unitOfWork.VehicleInformationRepository.IsNameUniqueAsync(command.VehicleName).Result)
        {
            return new InvalidResultNoData("Vehicle information is not unique.");
        }
        var data = _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeForVehicleInformationValidationQuery()).Result;
        var valdationData = new VehicleInformationValidationData(data);
        var result = _vehicleInformationFactory.CreateVehicleInformation(command, valdationData);
        if (result is InvalidResult<VehicleInformation>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleInformationRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithNoOperator command)
    { //trigger event VehicleAdded
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var validationData = new VehicleValidationData(vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, validationData);
        if (result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithOperators command) //remove
    { //trigger event VehicleAdded.
        var operators = _unitOfWork.OperatorRepository.AllAsync(new OperatorIdQuery()).Result;
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var valiationData = new VehicleValidationWithOperatorsData(operators, vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, valiationData);
        if (result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddDistanceToVehicleDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"Not found.");
        }
        BinaryFlag flag = entity.AddToDistanceMoved(command.DistanceToAdd);
        if (!flag)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(ResetVehicleMovedDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.OverwriteDistanceMoved(command.NewDistance);
        if (!flag)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishRelationBetweenOperatorAndVehicle command)
    { //raise some event that generates commands for the two handlers below
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(AddOperatorToVehicle command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddOperator(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(AddVehicleToOperator command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddVehicle(new(command.VehicleId));
        _unitOfWork.OperatorRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveRelationBetweenOperatorAndVehicle command)
    {
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(RemoveOperatorFromVehicle command)
    { 
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveOperator(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveVehicleFromOperator command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveVehicle(new(command.VehicleId));
        _unitOfWork.OperatorRepository.Update(entity);
        throw new NotImplementedException();
    }

    public Result Handle(StartOperatingVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StartOperating(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(StopOperatingVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"Not found.");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StopOperating(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromLicenseType command)
    {
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.LicenseTypeId).Result;
        if (entity is null)
        {
            //error most likely as this should only be called by an evnet.
        }
        entity.RemoveOperator(new(command.OperatorId));
        _unitOfWork.LicenseTypeRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(ValidateLicenseAgeRequirementBecauseChange command)
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
                //so some kind of recursive method (might not end up as a recursive method) that when event collection is empty make a check if there are new events. If there are run though them else return void.
                //this will also move the evnet publish code in UnitOfWork.Save() out of that method.
                //consider process manager for dealing with this.
                _unitOfWork.OperatorRepository.Update(entity);
                //the license type needs to know if a license is removed, so it can remove the operator
            }           
        }
        //tigger event LicenseTypeAgeRequirementValidatedSuccessed
        throw new NotImplementedException();
        return new SuccessResultNoData();
    }

    public Result Handle(ValidateLicenseRenewPeriodBecauseChange command)
    {
        //need a method on license to renew. Either that or on operator.
        //currently CheckIfExpired might be possible to recycle for this purpose.
        //if operator is not found trigger event for remove operator from licnese type.
        //if operator license type cannot be removed trigger event for removing them for license type and vehicld
        //at the end trigger LicenseTypeRenewPeriodValidatedSucessed
        throw new NotImplementedException();
    }

    public Result Handle(LicenseAgeRequirementRequireValidation command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if(entity is null)
        {
            _unitOfWork.AddOrphanEvnet(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return new InvalidResultNoData("Not Found.");
        }

        var license = entity.GetLicenseViaLicenseType(command.LicenseTypeId);
        if(license is null)
        {
            _unitOfWork.AddOrphanEvnet(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return new InvalidResultNoData("License Not Found.");
        }

        var status = entity.ValidateLicenseAgeRequirementIsFulfilled(command.NewAgeRequirement, command.LicenseTypeId);
        if (status == true)
        {
            entity.AddDomainEvent(new OperatorLicenseAgeRequirementValidated(entity, entity.Events.Count(), command.CorrelationId, command.CommandId)); ;
        }
        else
        {
            entity.RemoveLicense(license);
            entity.AddDomainEvent(new OperatorLicenseRetracted(entity, license, entity.Events.Count(), command.CorrelationId, command.CommandId)); ;
        }
        _unitOfWork.OperatorRepository.Update(entity);

        return new SuccessResultNoData();
    }

    public Result Handle(LicenseRenewPeriodRequireValidation command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            _unitOfWork.AddOrphanEvnet(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return new InvalidResultNoData("Not Found.");
        }

        var license = entity.GetLicenseViaLicenseType(command.LicenseTypeId);
        if (license is null)
        {
            _unitOfWork.AddOrphanEvnet(new OperatorForAgeValidatioNotFound(command.OperatorId, command.LicenseTypeId, command.CorrelationId, command.CausationId));
            return new InvalidResultNoData("License Not Found.");
        }

        var status = entity.ValidateLicenseRenewPeriodIsFulfilled(command.NewRenewPeriod, command.LicenseTypeId);
        if (status == true)
        {
            entity.AddDomainEvent(new OperatorLicenseRenewPeriodValidated(entity, entity.Events.Count(), command.CorrelationId, command.CommandId));
        }
        else
        {
            entity.AddDomainEvent(new OperatorLicenseExpired(entity, command.LicenseTypeId, entity.Events.Count(), command.CorrelationId, command.CommandId));
        }
        _unitOfWork.OperatorRepository.Update(entity);

        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperator command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        var removed = entity.RemoveOperator(new(command.OperatorId));
        entity.AddDomainEvent(removed ? new VehicleRemovedOperator(entity, command.OperatorId, command.CorrelationId, command.CommandId) : new VehicleNotRequiredToRemoveOperator(entity, command.CorrelationId, command.CommandId));

        _unitOfWork.VehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(FindVehicleInformationsWithSpecificLicenseType command)
    {
        var list = _unitOfWork.VehicleInformationRepository.FindAllWithSpecificLicenseTypeId(command.LicenseTypeId, new VehicleInformationIdQuery()).Result;
        _unitOfWork.AddOrphanEvnet(new FoundVehicleInformations(command.OperatorId, list.Select(x => x.Id), command.CorrelationId, command.CommandId));
        return new SuccessResultNoData();
    }

    public Result Handle(FindVehiclesWithSpecificVehicleInformationAndOperator command)
    {
        var list = _unitOfWork.VehicleRepository.FindSpecificByOperatorIdAndVehicleInformationsAsync(command.OperatorId, command.VehicleInformationIds, new VehicleIdQuery()).Result;
        _unitOfWork.AddOrphanEvnet(new VehiclesFoundWithSpecificVehicleInformationAndOperator(command.OperatorId, list.Select(x => x.Id), command.CorrelationId, command.CommandId));
        return new SuccessResultNoData();
    }
}
