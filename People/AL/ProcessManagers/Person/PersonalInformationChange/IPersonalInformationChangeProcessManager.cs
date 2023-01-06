using Common.ProcessManager;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
public interface IPersonalInformationChangeProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonPersonalInformationChangedSuccessed>,
    IProcessManagerEventHandler<PersonPersonalInformationChangedFailed>,
    IProcessManagerEventHandler<PersonAddedToGenderSucceeded>,
    IProcessManagerEventHandler<PersonRemovedFromGenderSucceeded>,
    IProcessManagerEventHandler<PersonChangedGender>, 
    IProcessManagerEventHandler<PersonAddedToGenderFailed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderFailed>
{
}
