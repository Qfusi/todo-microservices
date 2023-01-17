using Microsoft.EntityFrameworkCore;
using User.Service.Models;

namespace User.Service.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}