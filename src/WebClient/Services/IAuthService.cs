using GrpcUser;

namespace WebClient.Services;

public interface IAuthService
{
    bool IsLoggedIn();
    Task<bool> LoginAsync(LoginReq request);
    Task LogoutAsync();
    Task<bool> VerifyIsAuthenticatedAsync();

    /// <summary>
    /// Use this method when making any RPC call for baked in exception handling.
    /// </summary>
    Task<TResult?> InvokeRpcAsync<TResult>(Func<Task<TResult>> expression);
    event Action LoginStateChangeOccurred;
}