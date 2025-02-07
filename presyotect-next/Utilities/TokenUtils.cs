using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Presyotect.Utilities;

public static class TokenUtils
{
    public static string CreateToken(IConfiguration configuration, string username, params string[] roles)
    {
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

        List<Claim> claims =
        [
            new ("id", Guid.NewGuid().ToString()),
            new ("username", username),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        // Add all roles from account to the token
        if (roles.Length != 0)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}