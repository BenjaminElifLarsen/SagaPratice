using Common.RepositoryPattern;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
internal class PersonRepository : IPersonRepository
{
    private readonly IBaseRepository<Person> _baseRepository;

    public PersonRepository(IBaseRepository<Person> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<bool> DoesPersonExist(int id)
    {
        return !await _baseRepository.IsUniqueAsync(x => x == id);
    }

    public void Fire(Person entity)
    {
        _baseRepository.Delete(entity);
    }

    public void Hire(Person entity)
    {
        _baseRepository.Create(entity);
    }

    public void Save()
    {
        _baseRepository.SaveChanges();
    }

    public void UpdatePersonalInformation(Person entity)
    {
        _baseRepository.Update(entity);
    }
}
