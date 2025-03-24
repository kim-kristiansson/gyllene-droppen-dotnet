using GylleneDroppen.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
    }
}