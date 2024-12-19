using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BuildingBlocks.OpenApi;

public static class Extensions
{
    public static WebApplicationBuilder AddOpenApi(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.ConfigureOpenApi();
        return builder;
    }

    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services
            .AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        Array.Empty<string>()
                    }
                });
                // Add for nullable enum generate
                options.UseAllOfToExtendReferenceSchemas();
                options.SupportNonNullableReferenceTypes();
                // Add for enum name generate
                options.SchemaFilter<EnumSchemaFilter>();
            });
        services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
            })
            .EnableApiVersionBinding();

        // services.Configure<JsonOptions>(options =>
        //     options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))

        return services;
    }
    public static WebApplication UseOpenApi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "docker")
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.DisplayRequestDuration();
                
                // Build a swagger endpoint for each discovered API version
                var descriptions = app.DescribeApiVersions();
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
                
            });
        }
        return app;
    }
}
