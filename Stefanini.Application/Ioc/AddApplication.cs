using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Stefanini.Application.Ioc;

public static class Application
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        // services.AddScoped(
        //     typeof(IPipelineBehavior<,>),
        //     typeof(ValidationBehavior<,>));
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}