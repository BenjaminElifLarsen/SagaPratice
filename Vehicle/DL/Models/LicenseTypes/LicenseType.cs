﻿using Common.DDD;
using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
public class LicenseType : IAggregateRoot, ISoftDeleteDate
{
    private Guid _id;
    private string _type; //type can only be updated if there is no license that use the entity, need a query that look if any people got license with the specifc license type id
    private byte _renewPeriodInYears; //makes more sense to just use any(x => ...), in the repo, before trying to update, where should validation be done... entity? validator? handler? 
    private byte _ageRequirementInYears; //or would it make more sense that an incorrect, but not invalid, entity would be removed from the system and a new inserted?
    private DateOnly? _deletedFrom;
    private DateOnly _canBeIssuedFrom; //need to be put into ctor and validation, allow update as long time current date is not same or later as its value.
    private readonly HashSet<IdReference> _vehicleInformations;
    //cannot contain a collection of licenses, since License is not an aggregate root, could hold a collection of operators who got the required license.
    private readonly HashSet<IdReference> _operators;
    private HashSet<DomainEvent> _events;

    //internal int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    internal string Type { get => _type; private set => _type = value; }
    internal byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    internal byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }
    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }
    public DateOnly CanBeIssuedFrom { get => _canBeIssuedFrom; private set => _canBeIssuedFrom = value; } //can only be updated if there is no licenses that use it.
    public IEnumerable<Guid> VehicleInformations => _vehicleInformations.Select(x => x.Id);
    public IEnumerable<Guid> Operators => _operators.Select(x => x.Id);
    public Guid Id { get => _id; private set => _id = value; }

    public IEnumerable<DomainEvent> Events => _events;

    private LicenseType()
    {
        _events = new();
    }

    internal LicenseType(string type, byte renewPeriodInYears, byte ageRequirementInYears)
    {
        _id = Guid.NewGuid();
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
        _ageRequirementInYears = ageRequirementInYears;
        _vehicleInformations = new();
        _events = new();
        _operators = new();
    }

    internal void ReplaceType(string type)
    {
        _type = type;
    }

    internal void ChangeRenewPeriod(byte renewPeriod)
    {
        _renewPeriodInYears = renewPeriod;
    }

    internal void ChangeAgeRequirement(byte ageRequirement)
    {
        _ageRequirementInYears = ageRequirement;
    }

    public void Delete(DateOnly? dateTime)
    {
        _deletedFrom = dateTime;
    }

    internal bool AddVehicleInformation(Guid vehicleInformation)
    {
        return _vehicleInformations.Add(vehicleInformation);
    }


    internal bool AddOperator(Guid @operator)
    {
        return _operators.Add(@operator);
    }

    internal bool RemoveOperator(Guid @operator)
    {
        return _operators.Remove(@operator);
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }
}
