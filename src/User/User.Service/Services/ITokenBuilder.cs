using User.Service.Models;

namespace User.Service.Services;

public interface ITokenBuilder
{
    JwtToken GenerateToken(int username);
}