using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Admin.Api.Utilities;

public readonly struct ServiceResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ProblemDetails? ProblemDetails { get; init; }
    public string? RedirectUrl { get; init; }
    
    public static ServiceResponse<T> Success(T data) => new()
    {
        Data = data
    };

    public static ServiceResponse<T> Failure(string errorMessage, int statusCode) => new()
    {
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
}