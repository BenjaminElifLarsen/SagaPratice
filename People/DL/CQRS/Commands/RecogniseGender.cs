﻿using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class RecogniseGender : ICommand
{ //figure out a better name
    public string VerbSubject { get; private set; }
    public string VerbObject { get; private set; }
}