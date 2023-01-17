using GrpcUser;
using User.Service.Models;

namespace User.Service.Extensions;

public static class TodoMappingExtensions
{
    public static UserModel Map(this RegisterReq request)
    {
        return new UserModel
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };
    }
}