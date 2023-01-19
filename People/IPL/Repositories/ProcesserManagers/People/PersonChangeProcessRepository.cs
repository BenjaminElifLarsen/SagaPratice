using Common.RepositoryPattern.ProcessManagers;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
internal class PersonChangeProcessRepository : IPersonChangeProcessRepository
{
    private readonly IBaseProcessManagerRepository<PersonalInformationChangeProcessManager> _repository;

    public PersonChangeProcessRepository(IBaseProcessManagerRepository<PersonalInformationChangeProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<PersonalInformationChangeProcessManager> LoadAsync(Guid correlationId)
    {
        return await _repository.LoadAsync(correlationId);
    }

    public void Save(PersonalInformationChangeProcessManager manager)
    {
        _repository.Save(manager);
    }
}
