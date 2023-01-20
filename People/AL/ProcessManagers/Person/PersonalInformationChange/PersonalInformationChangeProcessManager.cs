using Common.BinaryFlags;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Events.Domain;
using static PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.PersonalInformationChangeProcessManager.ChangePersonStates;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
public sealed class PersonalInformationChangeProcessManager : BaseProcessManager, IPersonalInformationChangeProcessManager
{
    private bool? _genderRemovedProcessed;
    private bool? _genderAddedProccessed;
    private bool? _informationChangedProcessed;
    //private bool _genderChanged; //sat by PersonPersonalInformationChangedSuccessed.GenderWasChanged. 
    ////If false the PersonPersonalInformationChangedSuccessed handler can send out a 'finished' event else wait on gender added and gender removed has passed (need to check if they already have been handled or not)
    private BinaryFlag _binaryFlag; // In a relational database, like MSSQL, this would be stored as a numerical value.


    public PersonalInformationChangeProcessManager(Guid correlationId) : base(correlationId) 
    { //could log id
        ProcessManagerId = Guid.NewGuid();
        _binaryFlag = new(NotStarted);

    } //will need to handle state for removing gender event and adding gender event

    /*
     * when updating to use the events in Common.Events.Save make sure processingSucceeded is only called by PersonPersonalInformationChangedSuccessed if it should not wait on gender changes
     */

    public void Handle(PersonPersonalInformationChangedSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; } 

        //switch(_binaryFlag)
        //{
        //    case (long)NotStarted: //will not work as it does not check against the flag. This might be a case of an if-else structure solution is needed, yet it will break with the design of the other pms
        //        break; //this need to be well solved as the vehicle pms will require the solution a lot
        //}

        if(_binaryFlag == NotStarted)
        {
            _binaryFlag -= NotStarted;
            Processing();
            //if the binary flag only contains ChangedInformation the pm should place an event to let the caller know it is done with the processing
        }
        else if(_binaryFlag == ChangedGender)
        {
            Processing();
            //if the binary flag only contains ChangedGender and ChangedInformation and the gender operation booleans are true the pm should place an event for the caller
            //above statement is the logic of this event reaching the pm after the gender replacement event and the events for adding/removing genders have been processsed
            //if both gender operations succeeded it should place one specific event else another
        }
        else
        {
            //log
        }
        void Processing()
        {
            _binaryFlag += ChangedInformation;
            if (@event.GenderWasChanged)
            {
                //_genderChanged = true; 
                _genderAddedProccessed ??= false; // Gender specific operation event has not been processed yet, let the pm know that it need to wait on gender operation.
                _genderRemovedProcessed ??= false; //should this handler be able to set the waiting on gender states? That woud cause problems with the Handler(PersonReplacedGender).Processing()
                //currently these will be used for the purpose of letting this handler know if it can place an state event or not
            }
            if(_informationChangedProcessed != true)
            {
                _informationChangedProcessed = true;
                AddStateEvent(new InformationChangedSucceeded(@event.GenderWasChanged, CorrelationId, @event.EventId));
            }
        }
    }

    public void Handle(PersonPersonalInformationChangedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == NotStarted)
        {
            _binaryFlag -= NotStarted;
            _binaryFlag += FailedToChangeInformation;
            AddErrors(@event.Errors);
            AddStateEvent(new InformationChangedFailed(Errors, CorrelationId, @event.EventId));
        }
        else
        {

        }
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        if(@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == ChangedGender && _binaryFlag != AddedToGender)
        {
            Processing(); 
        }
        else
        {

        }

        void Processing()
        {
            _genderAddedProccessed = true;
            _binaryFlag -= WaitingOnGenderAdding;
            _binaryFlag += AddedToGender;
            if(_binaryFlag != WaitingOnGenderRemoving && _binaryFlag == ChangedInformation)
            {
                //transmit an event, event depends on if the removal failed or succeeded
                if (_binaryFlag == RemovedFromGender)
                {
                    AddStateEvent(new GenderReplacedSucceeded(CorrelationId, @event.EventId));
                }
                else if (_binaryFlag == FailedToBeRemovedFromGender)
                {
                    AddStateEvent(new GenderReplacedFailed(Errors, CorrelationId, @event.EventId));
                }
            }
        }
    }

    public void Handle(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == ChangedGender && _binaryFlag != FailedToBeAddedToGender)
        {
            Processing();
        }
        else
        {

        }

        void Processing()
        {
            _genderAddedProccessed = true;
            _binaryFlag -= WaitingOnGenderAdding;
            _binaryFlag += FailedToBeAddedToGender;
            AddErrors(@event.Errors);
            if (_binaryFlag != WaitingOnGenderRemoving && _binaryFlag == ChangedInformation)
            {
                //transmit an failer state event, does it matter to wait on the gender removal?
            }
        }
    }

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == ChangedGender && _binaryFlag != RemovedFromGender)
        {
            Processing();
        }
        else
        {

        }

        void Processing()
        {
            _genderRemovedProcessed = true;
            _binaryFlag -= WaitingOnGenderRemoving;
            _binaryFlag += RemovedFromGender;
            if (_binaryFlag != WaitingOnGenderAdding && _binaryFlag == ChangedInformation)
            {
                if (_binaryFlag == AddedToGender)
                {
                    AddStateEvent(new GenderReplacedSucceeded(CorrelationId, @event.EventId));
                }
                else if (_binaryFlag == FailedToBeAddedToGender)
                {
                    AddStateEvent(new GenderReplacedFailed(Errors, CorrelationId, @event.EventId));
                }
            }
        }
    }

    public void Handle(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == ChangedGender && _binaryFlag != FailedToBeRemovedFromGender)
        {
            Processing();
        }
        else
        {

        }

        void Processing()
        {
            _genderRemovedProcessed = true;
            _binaryFlag -= WaitingOnGenderRemoving;
            _binaryFlag += FailedToBeRemovedFromGender;
            AddErrors(@event.Errors);
            if (_binaryFlag != WaitingOnGenderAdding && _binaryFlag == ChangedInformation)
            {
                //transmit an failer state event, does it matter to wait on the gender removal?
            }
        }
    }

    public void Handle(PersonReplacedGender @event) //most likely a very good idea to create UML diagram for this entire pm
    { //currently either this or the PersonPersonalInformationChangedSuccessed can reach the pm first
        if (@event.CorrelationId != CorrelationId) { return; }

        if (_binaryFlag == NotStarted)
        {
            _binaryFlag -= NotStarted;
            Processing();
        }
        else if (_binaryFlag == ChangedInformation && _binaryFlag != ChangedGender)
        {
            Processing();
        }
        else
        {

        }

        void Processing()
        {
            _binaryFlag += ChangedGender;
            if (_binaryFlag != WaitingOnGenderAdding && _genderAddedProccessed != true)
            {
                AddCommand(new AddPersonToGender(@event.PersonId, @event.NewGenderId, @event.CorrelationId, @event.EventId));
                _binaryFlag += WaitingOnGenderAdding;
                _genderAddedProccessed = false;
            }
            if (_binaryFlag != WaitingOnGenderRemoving && _genderRemovedProcessed != true)
            {
                AddCommand(new RemovePersonFromGender(@event.PersonId, @event.OldGenderId, @event.CorrelationId, @event.EventId));
                _binaryFlag += WaitingOnGenderRemoving;
                _genderRemovedProcessed = false;
            }
        }
    }

    public enum ChangePersonStates
    {
        NotStarted = 0b1,
        ChangedInformation = 0b10,
        FailedToChangeInformation = 0b100,
        ChangedGender = 0b1000,
        AddedToGender = 0b10000,
        FailedToBeAddedToGender = 0b100000,
        RemovedFromGender = 0b1000000,
        FailedToBeRemovedFromGender = 0b10000000,
        WaitingOnGenderAdding = 0b100000000,
        WaitingOnGenderRemoving = 0b1000000000,

        Unknown = 0
    }
}
