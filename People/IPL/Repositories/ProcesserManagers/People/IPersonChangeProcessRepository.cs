using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
public interface IPersonChangeProcessRepository
{
    public Task<PersonalInformationChangeProcessManager> LoadAsync(Guid correlationId);
    public void Save(PersonalInformationChangeProcessManager manager);
    public void Delete(Guid correlationId);
}
