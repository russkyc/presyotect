using Presyotect.Features.Users.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Users.Services;

public class DevtestAuthenticator(IConfiguration configuration) : IAuthenticator
{
    public Validation Validate(User user)
    {
        var validation = new Validation();
        string[] allowed = ["admin", "personnel"];
        if (allowed.Any(username => user.Username.Equals(username)))
        {
            validation.Valid = true;
            validation.Message = "User Valid";
            return validation;
        }
        validation.Message = "User does not exist.";
        return validation;
    }

    public string Tokenize(User user)
    {
        return TokenUtils.CreateToken(configuration, user.Username, user.Username);
    }
}