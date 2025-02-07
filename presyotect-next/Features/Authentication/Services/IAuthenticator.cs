using Presyotect.Features.Authentication.Models;

namespace Presyotect.Features.Authentication.Services;

public interface IAuthenticator
{
    public Validation Validate(Account account);
    public string Tokenize(Account account);
}