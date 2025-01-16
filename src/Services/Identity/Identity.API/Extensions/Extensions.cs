using System.Reflection;
using Asp.Versioning.Conventions;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.OpenApi;
using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
using Identity.Application.Users.Abstractions;
using Identity.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Identity.Api.Extensions;

public static class Extensions
{
    private const string AllowAllOrigins = "AllowAll";
    
    public static IServiceCollection AddIdentityApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUserService, UserService>();
        
        var applicationAssembly = typeof(Program).Assembly;
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddMediatR(config =>
        {
            // config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.RegisterServicesFromAssembly(applicationAssembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: AllowAllOrigins,
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
        services.ConfigureOpenApi();
    
        return services;
    }

    public static WebApplication UseIdentityApiServices(this WebApplication app)
    {
        // register api versions
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(1)
            .HasApiVersion(2)
            .ReportApiVersions()
            .Build();

        // map versioned endpoint
        var versionGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        // use carter
        versionGroup.MapCarter();

        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/api/v1/identity/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        //Preserve Order - swaggerUI, always last
        app.UseCors(AllowAllOrigins);
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseOpenApi();
        
        return app;
    }
    
}