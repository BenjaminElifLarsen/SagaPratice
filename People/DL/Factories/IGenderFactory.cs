using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal interface IGenderFactory
{
    Result<Gender> CreateGender(RecogniseGender gender, GenderValidationData validationData); //figure out a better name
}
