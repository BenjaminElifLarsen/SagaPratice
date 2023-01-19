using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    private void Handler(ProcesserFinished @event) => _result = @event.Result;

    public void Handle(FiredSucceeded @event) { }

    public void Handle(FiredFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(RemovedFromGenderSucceeded @event) => _result = new SuccessResultNoData();

    public void Handle(RemovedFromGenderFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(HiredSucceeded @event) { }

    public void Handle(HiredFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(AddedToGenderSucceeded @event) => _result = new SuccessResultNoData();

    public void Handle(AddedToGenderFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(GenderReplacedSucceeded @event)
    {
        _genderChangeDone = true;
        if (!_genderChangeRequested)
            _genderChangeRequested = true;
        if (_informationChangeDone)
            _result = new SuccessResultNoData();
    }

    public void Handle(GenderReplacedFailed @event)
    {
            _result = new InvalidResultNoData(@event.Errors);
    }

    public void Handle(InformationChangedSucceeded @event)
    {
        _informationChangeDone = true;
        if (@event.GenderWasChanged)
            _genderChangeRequested = true;
        if (!_genderChangeRequested || (_genderChangeRequested && _genderChangeDone))
            _result = new SuccessResultNoData();
    }

    public void Handle(InformationChangedFailed @event)
    {
        _informationChangeDone = true;
        _result = new InvalidResultNoData(@event.Errors);
    }
}
