using Identity.Api.Extensions;
using Identity.Application;
using Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddIdentityApplicationServices(builder.Configuration)
    .AddIdentityApiServices(builder.Configuration);

builder.AddIdentityInfraServices();

var app = builder.Build();

app.UseIdentityApiServices();

app.Run();
