﻿using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
public interface IPersonalInformationChangeProcessManager : IBaseProcessManager,
    IProcessManagerEventHandler<PersonPersonalInformationChangedSuccessed>,
    IProcessManagerEventHandler<PersonPersonalInformationChangedFailed>,
    IProcessManagerEventHandler<PersonAddedToGenderSucceeded>,
    IProcessManagerEventHandler<PersonRemovedFromGenderSucceeded>,
    IProcessManagerEventHandler<PersonReplacedGender>, 
    IProcessManagerEventHandler<PersonAddedToGenderFailed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderFailed>
{
}
