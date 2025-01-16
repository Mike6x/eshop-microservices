using Asp.Versioning.Conventions;
using BuildingBlocks.OpenApi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.API;

public static class Extensions
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Add services to the container.
        var assembly = typeof(Program).Assembly;

        //register validators
        builder.Services.AddValidatorsFromAssembly(assembly);
        
        //register mediatr
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // register versioning and swagger
        builder.Services.ConfigureOpenApi();
        
        builder.Services.AddCarter();

        builder.Services.AddMarten(opts =>
        {
            opts.Connection(builder.Configuration.GetConnectionString("Database")!);    
        }).UseLightweightSessions();

        if (builder.Environment.IsDevelopment())
            builder.Services.InitializeMartenWith<CatalogInitialData>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddHealthChecks()
            .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

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

        app.UseHealthChecks("/api/v1/catalog/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        
        // register swaggerUI, always last
        app.UseOpenApi();
        
        return app;
    }
}
