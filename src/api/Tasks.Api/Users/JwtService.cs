using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tasks.Api.Users;

public interface IJwtService
{
    string GenerateToken(User user);
}

public class JwtService : IJwtService
{
    private readonly string issuer;
    private readonly string audience;
    private readonly SigningCredentials credentials;

    public JwtService(IConfiguration config)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        issuer = jwtSettings["Issuer"]!;
        audience = jwtSettings["Audience"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim("email", user.Email),
            new Claim("user_id", user.Id.ToString())
        };

        var tokenObject = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenObject);
    }
}