using User.Service.Models;

namespace User.Service.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public UserModel? GetByUsername(string username) => _context.Users.FirstOrDefault(x => x.Username == username);

    public bool Register(UserModel request)
    {
        var userExists = GetByUsername(request.Username);
        if (userExists != null)
            return false;

        _context.Users.Add(request);
        return SaveChanges();
    }

    private bool SaveChanges() => _context.SaveChanges() >= 0;
}