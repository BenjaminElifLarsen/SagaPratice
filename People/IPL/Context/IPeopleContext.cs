using BaseRepository;
using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.IPL.Context;
public interface IPeopleContext : IBaseContext, IContextData<Person>, IContextData<Gender>
{
}
