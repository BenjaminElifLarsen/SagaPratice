﻿using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Model;

public class Person : IAggregateRoot
{
    private int _personId;
    private string _firstName;
    private string _lastName;
    private DateOnly _birth;
    private IdReference _gender;

    internal int PersonId { get => _personId; private set => _personId = value; }
    internal string FirstName { get => _firstName; private set => _firstName = value; }
    internal string LastName { get => _lastName; private set => _lastName = value; }
    internal DateOnly Birth { get => _birth; private set => _birth = value; }
    internal IdReference Gender { get => _gender; private set => _gender = value; }

    private Person()
    {

    }

    internal Person(string firstName, string lastName, DateOnly birth, IdReference gender)
    {
        _firstName = firstName;
        _lastName = lastName;
        _birth = birth;
        _gender = gender;
    }

    internal Person(int personId, string firstName, string lastName, DateOnly birth, IdReference gender) : this(firstName, lastName, birth, gender)
    {
        _personId = personId;
    }

    internal void UpdateFistName(string firstName)
    {
        _firstName = firstName;
    }

    internal void UpdateLastName(string lastName)
    {
        _lastName = lastName;
    }

    internal void UpdateBirth(DateOnly birth)
    {
        _birth = birth;
    }

    internal void UpdateGender(IdReference gender)
    {
        _gender = gender;
    }

    public static bool operator ==(Person left, int right)
    {
        return left.PersonId == right;
    }

    public static bool operator !=(Person left, int right)
    {
        return !(left == right);
    }
}
