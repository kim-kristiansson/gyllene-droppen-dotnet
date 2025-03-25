namespace GylleneDroppen.Core.Common;

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public int? ErrorCode { get; }
    public string? RedirectUrl { get; }

    private Result(bool isSuccess, T? value, string? errorMessage, int? errorCode, string? redirectUrl)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        RedirectUrl = redirectUrl;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null, null, null);
    }

    public static Result<T> Failure(string message, int code)
    {
        return new Result<T>(false, default, message, code, null);
    }

    public static Result<T> Redirect(string url)
    {
        return new Result<T>(false, default, null, null, url);
    }
}