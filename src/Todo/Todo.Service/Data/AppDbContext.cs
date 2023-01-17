using Microsoft.EntityFrameworkCore;
using Todo.Service.Models;

namespace Todo.Service.Data;

public class AppDbContext : DbContext
{
    public DbSet<TodoModel> TodoEntries { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}