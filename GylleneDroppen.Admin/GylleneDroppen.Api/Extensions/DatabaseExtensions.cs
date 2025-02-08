using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Api.Extensions;

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