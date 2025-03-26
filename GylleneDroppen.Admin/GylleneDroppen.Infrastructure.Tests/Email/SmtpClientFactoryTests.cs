using System.Net;
using GylleneDroppen.Infrastructure.Cofiguration.Settings;
using GylleneDroppen.Infrastructure.Email;

namespace GylleneDroppen.Infrastructure.Tests.Email;

public class SmtpClientFactoryTests
{
    [Fact]
    public void Create_ShouldReturnCorrectlyConfiguredSmtpClient()
    {
        // Arrange
        var settings = new SmtpSettings
        {
            Host = "smtp.example.com",
            Port = 587,
            Username = "test@example.com",
            Password = "password123"
        };

        // Act
        var smtpClient = SmtpClientFactory.Create(settings);

        // Assert
        Assert.Equal(settings.Host, smtpClient.Host);
        Assert.Equal(settings.Port, smtpClient.Port);
        Assert.True(smtpClient.EnableSsl);

        var credentials = (NetworkCredential)smtpClient.Credentials!;
        Assert.Equal(settings.Username, credentials.UserName);
        Assert.Equal(settings.Password, credentials.Password);
    }
}