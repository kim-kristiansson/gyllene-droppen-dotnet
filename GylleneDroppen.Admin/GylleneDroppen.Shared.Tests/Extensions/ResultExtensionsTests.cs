using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Shared.Tests.Extensions;

public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_WithSuccessResult_ShouldReturnOkObjectResult()
    {
        // Arrange
        const string data = "Test Data";
        var result = Result<string>.Success(data);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(data, okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void ToActionResult_WithFailureResult_ShouldReturnProblemDetails()
    {
        // Arrange
        const string errorMessage = "Error occurred";
        const int statusCode = 400;
        var result = Result<string>.Failure(errorMessage, statusCode);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        Assert.Equal(statusCode, objectResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(errorMessage, problemDetails.Detail);
        Assert.Equal(statusCode, problemDetails.Status);
    }

    [Fact]
    public void ToActionResult_WithRedirectResult_ShouldReturnRedirectResult()
    {
        // Arrange
        const string redirectUrl = "https://example.com";
        var result = Result<string>.Redirect(redirectUrl);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(actionResult);
        Assert.Equal(redirectUrl, redirectResult.Url);
    }
}