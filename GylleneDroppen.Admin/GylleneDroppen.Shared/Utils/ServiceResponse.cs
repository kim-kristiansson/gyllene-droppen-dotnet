using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Shared.Utils;

public readonly struct ServiceResponse<T>
{
    private readonly ProblemDetails? _problemDetails;
    private readonly string? _redirectUrl;

    public bool IsSuccess { get; }
    public T? Data { get; }

    private ServiceResponse(bool isSuccess, T? data, ProblemDetails? problemDetails, string? redirectUrl)
    {
        IsSuccess = isSuccess;
        Data = data;
        _problemDetails = problemDetails;
        _redirectUrl = redirectUrl;
    }

    public static ServiceResponse<T> Success(T data)
    {
        return new ServiceResponse<T>(true, data, null, null);
    }

    public static ServiceResponse<T> Failure(string errorMessage, int statusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = errorMessage,
            Status = statusCode
        };

        return new ServiceResponse<T>(false, default, problemDetails, null);
    }

    public static ServiceResponse<T> Redirect(string redirectUrl)
    {
        return new ServiceResponse<T>(false, default, null, redirectUrl);
    }

    public IActionResult ToActionResult()
    {
        if (_redirectUrl != null)
            return new RedirectResult(_redirectUrl);

        return IsSuccess switch
        {
            false when _problemDetails != null => new ObjectResult(_problemDetails)
            {
                StatusCode = _problemDetails.Status ?? 500
            },
            true => new OkObjectResult(Data),
            _ => new StatusCodeResult(500)
        };
    }
}