using System.Reflection;
using GylleneDroppen.Admin.Api.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Admin.Api.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyConfigurations(this ModelBuilder modelBuilder)
    {
        var applyMethod = typeof(ModelBuilder)
            .GetMethods()
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration) &&
                        m.GetParameters().FirstOrDefault()?.ParameterType.GetGenericTypeDefinition() ==
                        typeof(IEntityTypeConfiguration<>));

        var configurations = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .Select(Activator.CreateInstance)
            .Where(instance => instance is not null) 
            .ToList();


        foreach (var config in configurations)
        {
            var entityType = config!.GetType().GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .GetGenericArguments()
                .First();

            var genericApplyMethod = applyMethod.MakeGenericMethod(entityType);
            genericApplyMethod.Invoke(modelBuilder, [config]);
        }
    }
}