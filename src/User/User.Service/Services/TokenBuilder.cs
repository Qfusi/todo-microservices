using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Service.Models;

namespace User.Service.Services;

public class TokenBuilder : ITokenBuilder
{
    private readonly string _jwtSigningKey;

    public TokenBuilder(IConfiguration configuration)
    {
        _jwtSigningKey = configuration["JwtSigningKey"]!;
    }

    public JwtToken GenerateToken(int id)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSigningKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
        };
        var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: DateTime.UtcNow.AddMinutes(10));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JwtToken(encodedJwt, jwt.ValidTo);
    }
}