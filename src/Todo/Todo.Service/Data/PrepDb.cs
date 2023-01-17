using Todo.Service.Models;

namespace Todo.Service.Data;

public static class PrepDb
{
    public static void PopulateDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
    }

    private static void SeedData(AppDbContext context)
    {
        if (!context.TodoEntries.Any())
        {
            context.TodoEntries.AddRange(
                new TodoModel { Title = "Do stuff", Text = "Do the stuff before tomorrow", Column = 1, CreatedAt = DateTime.UtcNow, OwnerId = 1 },
                new TodoModel { Title = "Do more stuff", Text = "Do the stuff before end of week", Column = 0, CreatedAt = DateTime.UtcNow, OwnerId = 1 },
                new TodoModel { Title = "Don't do any stuff at all", Text = "Don't do it", Column = 1, CreatedAt = DateTime.UtcNow, OwnerId = 2 },
                new TodoModel { Title = "Okay do some stuff", Text = "Not too much", Column = 2, CreatedAt = DateTime.UtcNow, OwnerId = 2 }
            );

            context.SaveChanges();
        }
    }
}