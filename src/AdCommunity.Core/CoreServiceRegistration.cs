﻿using AdCommunity.Core.Extensions;
using AdCommunity.Core.Extensions.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace AdCommunity.Core;

public static class CoreServiceRegistration
{
    public static IServiceCollection AddYt(this IServiceCollection services, params Assembly[] assemblies)
    {
        AddRequiredServices(services);
        RegisterServices(services, assemblies, typeof(IYtRequestHandler<,>));

        return services;
    }
    private static IServiceCollection RegisterServices(this IServiceCollection services, Assembly[] assemblies, Type registerationObj)
    {
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            var handlers = types.Where(x => x.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == registerationObj));
            foreach (var handle in handlers)
            {
                var interfaces = handle.GetInterfaces();
                foreach (var handleInterface in interfaces)
                {
                    services.AddTransient(handleInterface, handle);
                }
            }
        }

        return services;
    }

    private static void AddRequiredServices(IServiceCollection services)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IYtMediator), typeof(YtMediator), ServiceLifetime.Transient));
    }

}
