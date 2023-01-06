namespace PersonDomain.DL.Errrors;
internal enum GenderErrors
{
    CannotBeRemoved = 0b1,
    InvalidVerbObject = 0b10,
    InvalidVerbSubject = 0b100,
    VerbObjectAndSubjectInUse = 0b1000
}
