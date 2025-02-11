using Presyotect.Core.Contracts;

namespace Presyotect.Features.Users.Models;

public class Account : DbEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Nickname { get; set; }
    public string Role { get; set; }
}