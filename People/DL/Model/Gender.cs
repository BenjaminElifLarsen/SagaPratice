using Common.RepositoryPattern;
using PeopleDomain.DL.Models;
using System.Linq.Expressions;

namespace PeopleDomain.DL.Model;
internal class Gender : IAggregateRoot
{
    private int _genderId;
    private string _verbSubject;
    private string _verbObject;
    private readonly HashSet<IdReference> _people;

    public int GenderId { get => _genderId; private set => _genderId = value; }
    public string VerbSubject { get => _verbSubject; private set => _verbSubject = value; }
    public string VerbObject { get => _verbObject; private set => _verbObject = value; }
    public IEnumerable<IdReference> People { get => _people; }
    //what is the term for things like her/him/they and the term for she/him???

    private Gender()
    {

    }

    internal Gender(string subject, string @object)
    {
        _genderId = new Random(int.MaxValue).Next();
        _people = new();
        _verbSubject = subject;
        _verbObject = @object;
    }

    //public void UpdateName(string name)
    //{ //does not make sense to change subject or object
    //    _verbSubject = name;
    //}

    public bool AddPerson(IdReference person)
    {
        return _people.Add(person);
    }

    //public IEnumerable<Person> GetSpecificPeople(params Expression<Func<Person, bool>>[] predicates)
    //{ //if using IdReference this will not be useful 
    //    var query = _people.AsQueryable();
    //    query = predicates.Aggregate(query, (source, where) => source.Where(where));
    //    return query;
    //}

    public static bool operator ==(Gender left, int right)
    {
        return left.GenderId == right;
    }

    public static bool operator !=(Gender left, int right)
    {
        return !(left == right);
    }

    public static bool operator ==(Gender left, Gender right)
    {
        return left.GenderId == right.GenderId;
    }

    public static bool operator !=(Gender left, Gender right)
    {
        return !(left == right);
    }
}