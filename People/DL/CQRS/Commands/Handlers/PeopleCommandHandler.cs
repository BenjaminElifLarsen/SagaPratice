using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Queries;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.DL.CQRS.Commands.Handlers;
internal class PeopleCommandHandler : IPeopleCommandHandler
{
    private readonly IPersonFactory _personFactory;
    private readonly IPersonRepository _personRepository;

    private readonly IGenderFactory _genderFactory;
    private readonly IGenderRepository _genderRepository;

    public PeopleCommandHandler(IPersonFactory personFactory, IPersonRepository personRepository, IGenderFactory genderFactory, IGenderRepository genderRepository)
    {
        _personFactory = personFactory;
        _personRepository = personRepository;
        _genderFactory = genderFactory;
        _genderRepository = genderRepository;
    }

    public Result Handle(HirePersonFromUser command)
    { //trigger event. Gender will also need to know of the new person
        var genderIds = _genderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        var result = _personFactory.CreatePerson(command, validationData);
        if(result is InvalidResult<Person>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _personRepository.Hire(result.Data);
        _personRepository.Save();
        return new SuccessResultNoData();
    } //maybe move all of the handler implementation code into domain services, one for each aggregate roots

    public Result Handle(FirePersonFromUser command)
    { //trigger event
        var entity = _personRepository.GetForOperationAsync(command.Id).Result;
        if(entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            _personRepository.Fire(entity);
            _personRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    { //trigger event only if birth was changed
        var entity = _personRepository.GetForOperationAsync(command.Id).Result;
        if(entity is null)
        {
            return new InvalidResultNoData("");
        }
        
        throw new NotImplementedException();
    }

    public Result Handle(RecogniseGender command)
    {
        var genders = _genderRepository.AllAsync(new GenderVerbQuery()).Result;
        var validationData = new GenderValidationData(genders);
        var result = _genderFactory.CreateGender(command, validationData);
        if(result is InvalidResult<Gender>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _genderRepository.Recognise(result.Data);
        _genderRepository.Save();
        return new SuccessResultNoData();
    }
}
