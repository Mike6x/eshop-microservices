using Identity.Application.Users.Dtos;
using Identity.Domain.Users;
using Mapster;

namespace Identity.Application.Users.Mappings;

public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, UserDto>();
    }
}
