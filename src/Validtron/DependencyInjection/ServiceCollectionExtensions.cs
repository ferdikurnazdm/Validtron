using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Validtron.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidtron(this IServiceCollection services, Assembly assembly)
    {
        var validatorDefinition = typeof(IValidator<>);

        var implementationTypes = assembly
            .GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                !type.IsGenericTypeDefinition);

        foreach (var implementationType in implementationTypes)
        {
            var validatorInterfaces = implementationType
                .GetInterfaces()
                .Where(@interface =>
                    @interface.IsGenericType &&
                    @interface.GetGenericTypeDefinition() == validatorDefinition);

            foreach (var serviceType in validatorInterfaces)
            {
                _ = services.AddScoped(serviceType, implementationType);
            }
        }

        return services;
    }
}