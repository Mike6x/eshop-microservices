using System.Net;
using BuildingBlocks.Exceptions;

namespace Identity.Application.Users.Exceptions;
public class UserRegistrationException : CustomException
{
    public UserRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
    {
    }
}
