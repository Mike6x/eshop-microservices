using Identity.Application.Users.Features.RegisterUser;

namespace Identity.Application.Users.Abstractions;

public interface IUserService
{
    Task<RegisterUserResponse> RegisterAsync(RegisterUserCommand request, string origin, CancellationToken cancellationToken);
    
}