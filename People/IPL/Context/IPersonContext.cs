using BaseRepository;
using Common.RepositoryPattern;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Context;
public interface IPersonContext : IBaseContext, IContextData<Person>, IContextData<Gender>
{
}
