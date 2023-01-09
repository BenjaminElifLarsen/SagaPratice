using Common.Events.Domain;
using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal interface IGenderFactory
{
    Result<Gender> CreateGender(RecogniseGender gender, GenderValidationData validationData); //figure out a better name
    Gender HydrateGender(IEnumerable<DomainEvent> events);
}
