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

    public void Fire(Person person)
    {
        _baseRepository.Delete(person);
    }

    public void Hire(Person person)
    {
        _baseRepository.Create(person);
    }

    public void Save()
    {
        _baseRepository.SaveChanges();
    }

    public void UpdatePersonalInformation(Person person)
    {
        _baseRepository.Update(person);
    }
}
