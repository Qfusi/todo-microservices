using User.Service.Models;

namespace User.Service.Data;

public static class PrepDb
{
    public static void PopulateDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
    }

    private static void SeedData(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new UserModel { Username = "user1", Password = "pass1", Email = "test@email.com" },
                new UserModel { Username = "user2", Password = "pass2", Email = "test@email.com" }
            );

            context.SaveChanges();
        }
    }
}