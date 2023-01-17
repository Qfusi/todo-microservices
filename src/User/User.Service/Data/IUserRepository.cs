using GrpcUser;
using User.Service.Models;

namespace User.Service.Data;

public interface IUserRepository
{
    UserModel? GetByUsername(string username);
    bool Register(UserModel request);
}