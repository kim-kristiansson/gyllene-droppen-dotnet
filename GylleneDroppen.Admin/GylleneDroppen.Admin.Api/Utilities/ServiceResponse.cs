using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Admin.Api.Utilities;

public readonly struct ServiceResponse<T>
{
    private bool IsSuccess { get; init; }
    private T? Data { get; init; }
    private ProblemDetails? ProblemDetails { get; init; }
    private string? RedirectUrl { get; init; }
    
    public static ServiceResponse<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ServiceResponse<T> Failure(string errorMessage, int statusCode) => new()
    {
        IsSuccess = false,
        ProblemDetails = new ProblemDetails
        {
            Detail = errorMessage,
            Status = statusCode
        }
    };

    public static ServiceResponse<T> Redirect(string redirectUrl) => new()
    {
        RedirectUrl = redirectUrl
    };
    
    public IActionResult ToActionResult()
    {
        if(RedirectUrl != null)
            return new RedirectResult(RedirectUrl);

        return IsSuccess switch
        {
            false when ProblemDetails != null => new ObjectResult(ProblemDetails)
            {
                StatusCode = ProblemDetails.Status ?? 500
            },
            true => new OkObjectResult(Data),
            _ => new StatusCodeResult(500)
        };
    }
}