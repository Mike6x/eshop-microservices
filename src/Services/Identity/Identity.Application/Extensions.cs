using System.Reflection;
using BuildingBlocks.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Identity.Application;

public static class Extensions
{
    public static IServiceCollection AddIdentityApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // var applicationAssembly = typeof(IdentityCore).Assembly;
        // services.AddValidatorsFromAssembly(applicationAssembly);

        // services.AddMediatR(config =>
        // {
        //     // config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        //     config.RegisterServicesFromAssembly(applicationAssembly);
        //     config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        //     config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        // });

        services.AddFeatureManagement();
       // services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}