using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class RecogniseGender : ICommand
{ //figure out a better name
    public string VerbSubject { get; set; }
    public string VerbObject { get; set; }
}
