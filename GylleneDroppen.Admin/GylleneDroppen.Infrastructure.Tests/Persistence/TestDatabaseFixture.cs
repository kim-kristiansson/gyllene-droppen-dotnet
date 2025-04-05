using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Tests.Persistence;

/// <summary>
///     A shared fixture to provide PostgreSQL database access for tests
/// </summary>
public class TestDatabaseFixture : IAsyncLifetime
{
    private readonly IContainer _postgresContainer;

    public TestDatabaseFixture()
    {
        // Configure PostgreSQL container
        _postgresContainer = new ContainerBuilder()
            .WithImage("postgres:16")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithEnvironment("POSTGRES_DB", "testdb")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public DbContextOptions<AppDbContext> ContextOptions { get; private set; }

    public async Task InitializeAsync()
    {
        // Start the container
        await _postgresContainer.StartAsync();

        // Get the mapped port
        var mappedPort = _postgresContainer.GetMappedPublicPort(5432);

        // Configure DbContext to use the container
        ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql($"Host=localhost;Port={mappedPort};Database=testdb;Username=postgres;Password=postgres")
            .Options;

        // Create the schema
        using var context = new AppDbContext(ContextOptions);
        await context.Database.EnsureDeletedAsync(); // Start with a clean database
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        if (_postgresContainer != null)
        {
            await _postgresContainer.StopAsync();
            await _postgresContainer.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}