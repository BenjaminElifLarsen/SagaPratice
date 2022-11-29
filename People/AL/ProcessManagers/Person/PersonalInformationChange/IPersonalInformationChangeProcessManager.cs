using Common.ProcessManager;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
public interface IPersonalInformationChangeProcessManager : IProcessManager,
    IProcessManagerEventHandler<PersonPersonalInformationChangedSuccessed>,
    IProcessManagerEventHandler<PersonPersonalInformationChangedFailed>,
    IProcessManagerEventHandler<PersonAddedToGenderSuccessed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderSuccessed>,
    IProcessManagerEventHandler<PersonChangedGender>, 
    IProcessManagerEventHandler<PersonAddedToGenderFailed>,
    IProcessManagerEventHandler<PersonRemovedFromGenderFailed>
{
}
