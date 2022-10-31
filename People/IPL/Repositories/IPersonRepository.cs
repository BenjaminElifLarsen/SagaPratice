using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
internal interface IPersonRepository
{
    public void Hire(Person entity);
    public void Fire(Person entity);
    public void UpdatePersonalInformation(Person entity);
    public void Save();
    public Task<bool> DoesPersonExist(int id);
    Task<Person> GetForOperationAsync(int id);
}
