using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Repositories;
internal interface IPersonRepository
{
    public void Hire(Person person);
    public void Fire(Person person);
    public void UpdatePersonalInformation(Person person);
    public void Save();
    public Task<bool> DoesPersonExist(int id);
}
