using GylleneDroppen.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Shared.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.RedirectUrl != null)
            return new RedirectResult(result.RedirectUrl);

        if (!result.IsSuccess)
            return new ObjectResult(new ProblemDetails
            {
                Detail = result.ErrorMessage,
                Status = result.ErrorCode
            })
            {
                StatusCode = result.ErrorCode
            };

        return new OkObjectResult(result.Value);
    }
}