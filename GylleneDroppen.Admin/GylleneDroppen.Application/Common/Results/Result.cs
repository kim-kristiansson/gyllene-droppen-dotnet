namespace GylleneDroppen.Application.Common.Results;

public readonly struct Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public int? StatusCode { get; init; }
    public string? RedirectUrl { get; init; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    public static Result<T> Failure(string message, int code)
    {
        return new Result<T> { IsSuccess = false, ErrorMessage = message, StatusCode = code };
    }

    public static Result<T> Redirect(string url)
    {
        return new Result<T> { RedirectUrl = url };
    }
}