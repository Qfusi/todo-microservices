using Grpc.Core;
using GrpcUser;
using User.Service.Data;
using User.Service.Extensions;
using User.Service.Models;
using static GrpcUser.User;

namespace User.Service.Services;

public class UserService : UserBase
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITokenBuilder _tokenBuilder;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository, ITokenBuilder tokenBuilder)
    {
        _logger = logger;
        _userRepository = userRepository;
        _tokenBuilder = tokenBuilder;
    }

    public override Task<LoginRsp> Login(LoginReq request, ServerCallContext context)
    {
        _logger.LogInformation("--> Received request: {username} - {password}", request.Username, request.Password);

        var user = _userRepository.GetByUsername(request.Username);
        if (user == null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        if (user.Password != request.Password)
            throw new RpcException(new Status(StatusCode.NotFound, "Incorrect password"));

        var token = _tokenBuilder.GenerateToken(user.Id);

        _logger.LogInformation("--> Login successful.");
        return Task.FromResult(token.Map());
    }

    public override Task<RegisterRsp> Register(RegisterReq request, ServerCallContext context)
    {
        _logger.LogInformation("--> Received request: {username} - {password}", request.Username, request.Password);

        var success = _userRepository.Register(request.Map());

        return Task.FromResult(new RegisterRsp { Success = success });
    }
}
