using Presyotect.Core.Models;

namespace Presyotect.Features.Authentication.Models;

public class Account : DbEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string[] Roles { get; set; }
}