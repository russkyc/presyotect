using Presyotect.Features.Users.Models;

namespace Presyotect.Features.Users.Services;

public interface IAuthenticator
{
    public Validation Validate(User user);
    public string Tokenize(User user);
}