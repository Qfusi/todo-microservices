using Grpc.Core;
using GrpcUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using static GrpcUser.User;

namespace Gateway.Service.Services;

public class UserService : UserBase
{
    private readonly ILogger<TodoService> _logger;
    private readonly UserClient _userClient;

    public UserService(ILogger<TodoService> logger, UserClient client)
    {
        _logger = logger;
        _userClient = client;
    }

    public override async Task<LoginRsp> Login(LoginReq request, ServerCallContext context)
    {
        _logger.LogInformation("--> Routing Login request to User-Service: {username} - {password}", request.Username, request.Password);

        var response = await _userClient.LoginAsync(request);

        _logger.LogInformation("--> Routing response to client with token: {token}", response.JwtToken);

        return response;
    }

    public override async Task<RegisterRsp> Register(RegisterReq request, ServerCallContext context)
    {
        _logger.LogInformation("--> Routing Register request to User-Service: {username} - {password}", request.Username, request.Password);

        var response = await _userClient.RegisterAsync(request);

        _logger.LogInformation("--> Routing response to client with token: {token}", response.Success);

        return response;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public override Task<VerifyRsp> VerifyAuthenticated(VerifyReq request, ServerCallContext context) =>
        // No handling required, endpoint just need to exist since token verification has already been handled in authentication middleware.
        Task.FromResult(new VerifyRsp());
}
