using System.Security.Claims;
using Identity.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Controllers;

[ApiController]
public class TokensController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public TokensController(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var oidcRequest = HttpContext.GetOpenIddictServerRequest() ?? throw new ArgumentNullException();
        if (oidcRequest.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsGrantType(oidcRequest);
        }

        if (oidcRequest.IsPasswordGrantType())
        {
           // return await TokensForPasswordGrantType(oidcRequest);
        }
        
        if (oidcRequest.IsRefreshTokenGrantType())
        {
            // return tokens for refresh token flow
        }

        if (oidcRequest.GrantType == "custom_flow_name")
        {
            // return tokens for custom flow
        }
        throw new NotImplementedException("The specified grant type is not implemented.");
    }

    private async Task<IActionResult> HandleClientCredentialsGrantType(OpenIddictRequest? request)
    {
        object? application = await _applicationManager.FindByClientIdAsync(request!.ClientId!) ?? throw new InvalidOperationException("The application details cannot be found in the database.");
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);
        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));
        identity.SetScopes(request!.GetScopes());
        identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
        identity.SetDestinations(GetDestinations);
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or
            Claims.Subject
                => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken },
        };
    }
}
