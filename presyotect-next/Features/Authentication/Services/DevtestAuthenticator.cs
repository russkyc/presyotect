using Presyotect.Features.Authentication.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Authentication.Services;

public class DevtestAuthenticator(IConfiguration configuration) : IAuthenticator
{
    public Validation Validate(Account account)
    {
        var validation = new Validation();
        string[] allowed = ["admin", "personnel"];
        if (allowed.Any(username => account.Username.Equals(username)))
        {
            validation.Valid = true;
            validation.Message = "Account Valid";
            return validation;
        }
        validation.Message = "Account does not exist.";
        return validation;
    }

    public string Tokenize(Account account)
    {
        return TokenUtils.CreateToken(configuration, account.Username, account.Username);
    }
}