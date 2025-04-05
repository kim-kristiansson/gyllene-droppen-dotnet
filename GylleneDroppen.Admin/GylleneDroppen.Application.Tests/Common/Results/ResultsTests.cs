using GylleneDroppen.Application.Common.Results;

namespace GylleneDroppen.Application.Tests.Common.Results;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Arrange
        const string data = "Test Data";

        // Act
        var result = Result<string>.Success(data);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(data, result.Data);
        Assert.Null(result.ErrorMessage);
        Assert.Null(result.StatusCode);
        Assert.Null(result.RedirectUrl);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        // Arrange
        const string errorMessage = "Error occurred";
        const int statusCode = 400;

        // Act
        var result = Result<string>.Failure(errorMessage, statusCode);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.ErrorMessage);
        Assert.Equal(statusCode, result.StatusCode);
        Assert.Null(result.Data);
        Assert.Null(result.RedirectUrl);
    }

    [Fact]
    public void Redirect_ShouldCreateRedirectResult()
    {
        // Arrange
        const string redirectUrl = "https://example.com";

        // Act
        var result = Result<string>.Redirect(redirectUrl);

        // Assert
        Assert.Equal(redirectUrl, result.RedirectUrl);
        Assert.Null(result.Data);
        Assert.Null(result.ErrorMessage);
        Assert.Null(result.StatusCode);
    }
}