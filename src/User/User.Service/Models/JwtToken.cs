using GrpcUser;

namespace User.Service.Models;

public record JwtToken(string Token, DateTime Expiration);

public static class JwtExtensions
{
    public static LoginRsp Map(this JwtToken token)
    {
        return new LoginRsp
        {
            JwtToken = token.Token,
            Expiration = token.Expiration.ToString()
        };
    }
}