using System.Reflection;
using GylleneDroppen.Infrastructure.Cofiguration.Extensions;
using GylleneDroppen.Infrastructure.Cofiguration.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Infrastructure.Tests.Configuration.Extensions;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void AddApplicationConfiguration_RegistersStandardSettings()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string>
        {
            { "DatabaseSettings:ConnectionString", "Host=localhost;Database=testdb;Username=user;Password=pass" },
            { "JwtSettings:Secret", "TestSecretKey1234567890TestSecretKey" },
            { "JwtSettings:Issuer", "test-issuer" },
            { "JwtSettings:Audience", "test-audience" },
            { "JwtSettings:AccessTokenExpirationMinutes", "60" },
            { "JwtSettings:RefreshTokenExpirationDays", "30" },
            { "GlobalSettings:ApiBaseUrl", "http://localhost:5000" },
            { "GlobalSettings:FrontendBaseUrl", "http://localhost:3000" },
            { "GlobalSettings:AdminBaseUrl", "http://localhost:5090" },
            { "RedisSettings:ConnectionString", "localhost:6379" },
            { "RedisSettings:InstanceName", "TestInstance" },
            { "SmtpSettings:Host", "smtp.test.com" },
            { "SmtpSettings:Port", "587" },
            { "SmtpSettings:Username", "test@example.com" },
            { "SmtpSettings:Password", "test-password" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        // Act
        services.AddApplicationConfiguration(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert - Test that all standard settings are registered and values are bound correctly
        var dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>()?.Value;
        var jwtSettings = serviceProvider.GetService<IOptions<JwtSettings>>()?.Value;
        var globalSettings = serviceProvider.GetService<IOptions<GlobalSettings>>()?.Value;
        var redisSettings = serviceProvider.GetService<IOptions<RedisSettings>>()?.Value;
        var smtpSettings = serviceProvider.GetService<IOptions<SmtpSettings>>()?.Value;

        // Verify settings are registered
        Assert.NotNull(dbSettings);
        Assert.NotNull(jwtSettings);
        Assert.NotNull(globalSettings);
        Assert.NotNull(redisSettings);
        Assert.NotNull(smtpSettings);

        // Verify values are bound correctly
        Assert.Equal("Host=localhost;Database=testdb;Username=user;Password=pass", dbSettings.ConnectionString);
        Assert.Equal("TestSecretKey1234567890TestSecretKey", jwtSettings.Secret);
        Assert.Equal("test-issuer", jwtSettings.Issuer);
        Assert.Equal(60, jwtSettings.AccessTokenExpirationMinutes);
        Assert.Equal(30, jwtSettings.RefreshTokenExpirationDays);
        Assert.Equal("http://localhost:5000", globalSettings.ApiBaseUrl);
        Assert.Equal("localhost:6379", redisSettings.ConnectionString);
        Assert.Equal("TestInstance", redisSettings.InstanceName);
        Assert.Equal("smtp.test.com", smtpSettings.Host);
        Assert.Equal(587, smtpSettings.Port);
    }

    [Fact]
    public void AddApplicationConfiguration_RegistersAdditionalSettings_FromCallingAssembly()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string>
        {
            { "TestSettings:TestValue", "test123" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        // Act
        services.AddApplicationConfiguration(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert - Test that additional settings from test assembly are registered
        var testSettings = serviceProvider.GetService<IOptions<TestSettings>>()?.Value;

        // Verify it was auto-discovered
        Assert.NotNull(testSettings);
        Assert.Equal("test123", testSettings.TestValue);
    }

    [Fact]
    public void AddConfigurationFromAssembly_RegistersSettings_FromSpecificAssembly()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string>
        {
            { "AnotherTestSettings:AnotherValue", "another-test-value" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        // Act - Register from current assembly explicitly
        services.AddConfigurationFromAssembly(configuration, Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var anotherTestSettings = serviceProvider.GetService<IOptions<AnotherTestSettings>>()?.Value;

        Assert.NotNull(anotherTestSettings);
        Assert.Equal("another-test-value", anotherTestSettings.AnotherValue);
    }

    [Fact]
    public void AddApplicationConfiguration_HandlesEmptyOrMissingValues()
    {
        // Arrange
        var services = new ServiceCollection();
        var configValues = new Dictionary<string, string>
        {
            { "DatabaseSettings:ConnectionString", "" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        // Act
        services.AddApplicationConfiguration(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert - Test handling of empty values
        var dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>()?.Value;

        Assert.NotNull(dbSettings);
        Assert.Equal("", dbSettings.ConnectionString);
    }

    [Fact]
    public void AddApplicationConfiguration_HandlesJsonFileConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();

        // Create a temporary test JSON config file
        var configJson = @"{
            ""DatabaseSettings"": {
                ""ConnectionString"": ""Host=localhost;Database=jsontest;Username=jsonuser;Password=jsonpass""
            }
        }";

        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, configJson);

        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(tempFile)
                .Build();

            // Act
            services.AddApplicationConfiguration(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var dbSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>()?.Value;

            Assert.NotNull(dbSettings);
            Assert.Equal("Host=localhost;Database=jsontest;Username=jsonuser;Password=jsonpass",
                dbSettings.ConnectionString);
        }
        finally
        {
            // Clean up
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void AddApplicationConfiguration_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var result = services.AddApplicationConfiguration(configuration);

        // Assert - Confirm it returns the same instance for method chaining
        Assert.Same(services, result);
    }
}

public class TestSettings
{
    public required string TestValue { get; init; }
}

public class AnotherTestSettings
{
    public required string AnotherValue { get; init; }
}