using GylleneDroppen.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GylleneDroppen.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        var databaseSettings = new DatabaseSettings
        {
            ConnectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value
        };

        if (string.IsNullOrEmpty(databaseSettings.ConnectionString))
            throw new InvalidOperationException(
                "Could not find a connection string. Please ensure DatabaseSettings:ConnectionString is set in appsettings.json");

        // Configure the DbContext using the same pattern as your application
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(databaseSettings.ConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}