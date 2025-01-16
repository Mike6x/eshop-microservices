using Identity.Application.Users.Abstractions;
using Identity.Application.Users.Features.RegisterUser;

namespace Identity.Api.Endpoints.Users;

public static class RegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/register", (RegisterUserCommand request,
                IUserService service,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
                return service.RegisterAsync(request, origin, cancellationToken);
            })
            .WithName(nameof(RegisterUserEndpoint))
            .WithSummary("register user")
           // .RequirePermission("Permissions.Users.Create")
            .WithDescription("register user");
    }
}