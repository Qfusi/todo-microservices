using Blazored.LocalStorage;
using Grpc.Core;
using GrpcUser;
using Microsoft.AspNetCore.Components;
using WebClient.Constants;

namespace WebClient.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<IAuthService> _logger;
    private readonly ILocalStorageService _localStorage;
    private readonly User.UserClient _userClient;
    private readonly NavigationManager _navigation;
    private bool _loggedIn;

    public AuthService(
        ILogger<IAuthService> logger,
        ILocalStorageService localStorage,
        User.UserClient userClient,
        NavigationManager navigation)
    {
        _logger = logger;
        _localStorage = localStorage;
        _userClient = userClient;
        _navigation = navigation;
    }

    public bool IsLoggedIn() => _loggedIn;

    public async Task<bool> LoginAsync(LoginReq request)
    {
        try
        {
            var response = await _userClient.LoginAsync(request);
            await _localStorage.SetItemAsStringAsync(LocalStorageConstants.TokenKey, response!.JwtToken);
            _loggedIn = true;
            NotifyLoginStateHasChanged();
            _navigation.NavigateTo("/");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(LocalStorageConstants.TokenKey);
        _loggedIn = false;
        _navigation.NavigateTo("/");
        NotifyLoginStateHasChanged();
    }

    public async Task<bool> VerifyIsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(LocalStorageConstants.TokenKey);

        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            await _userClient.VerifyAuthenticatedAsync(new VerifyReq());
            if (!_loggedIn)
            {
                _loggedIn = true;
                NotifyLoginStateHasChanged();
            }
            _logger.LogInformation("Jwt token is still valid");
        }
        catch (System.Exception)
        {
            _logger.LogInformation("Jwt token has expired");
            if (_loggedIn)
            {
                _loggedIn = false;
                NotifyLoginStateHasChanged();
            }
        }
        return _loggedIn;
    }

    public async Task<TResult?> InvokeRpcAsync<TResult>(Func<Task<TResult>> expression)
    {
        TResult? result = default;
        try
        {
            result = await expression();
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == StatusCode.Unauthenticated)
            {
                await _localStorage.RemoveItemAsync(LocalStorageConstants.TokenKey);
                _loggedIn = false;
                _navigation.NavigateTo("/");
            }
            NotifyLoginStateHasChanged();
        }
        return result;
    }

    public event Action LoginStateChangeOccurred;
    private void NotifyLoginStateHasChanged() => LoginStateChangeOccurred?.Invoke();
}