using Carter;
using Identity.Api.Endpoints.Users;

namespace Identity.Api.Endpoints;

public class IdentityModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("identites") { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("identites").WithTags("User's API Group");

            userGroup.MapRegisterUserEndpoint();


        }
    }
}