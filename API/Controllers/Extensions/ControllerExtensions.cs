using Common.ResultPattern;
using Common.ResultPattern.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Extensions;

public static class ControllerExtensions
{
    public static ActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
    {
        return result.ResultType switch
        {
            ResultTypes.Success => controller.Ok(result.Data),
            ResultTypes.SuccessNotData => controller.NoContent(),
            ResultTypes.Invalid => controller.BadRequest(result.Errors),
            ResultTypes.NotFound => controller.NotFound(result.Errors),
            ResultTypes.Unexpected => controller.BadRequest(result.Errors),
            _ => throw new Exception("An unhandled result has occured. Please inform the developers that something has occured."), // Only thrown if the int value of ResultType is not a part of the ResultTypes. This should never be hit. 
        };
    }
}
