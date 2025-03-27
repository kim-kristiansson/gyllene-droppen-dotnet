using GylleneDroppen.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Presentation.Utilities;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.RedirectUrl != null)
            return new RedirectResult(result.RedirectUrl);

        return result.IsSuccess switch
        {
            false when result.StatusCode.HasValue => new ObjectResult(new ProblemDetails
            {
                Detail = result.ErrorMessage, Status = result.StatusCode.Value
            }) { StatusCode = result.StatusCode.Value },
            true => new OkObjectResult(result.Data),
            _ => new StatusCodeResult(500)
        };
    }
}