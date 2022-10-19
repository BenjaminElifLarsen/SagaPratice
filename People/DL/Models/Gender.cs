using Common.RepositoryPattern;
using System.Linq.Expressions;

namespace People.DL.Models;
internal class Gender : IAggregateRoot
{
    private int _genderId;
    private string _name;
    private HashSet<Person> _people;

    public int GenderId { get => _genderId; private set => _genderId = value; }
    public string Name { get => _name; private set => _name = value; }
    public IEnumerable<Person> People { get => _people;}
    //what is the term for things like her/him/they and the term for she/him???
    private Gender()
    {

    }

    internal Gender(int genderId, string name)
    {
        _genderId = genderId;
        _name = name;
        _people = new HashSet<Person>();
    }

    public void UpdateName(string name)
    {
        _name = name;
    }

    public bool AddPerson(Person person)
    {
        return _people.Add(person);
    }

    public IEnumerable<Person> GetSpecificPeople(params Expression<Func<Person, bool>>[] predicates)
    {
        var query = _people.AsQueryable();
        query = predicates.Aggregate(query, (source, where) => source.Where(where));
        return query;
    }
}
