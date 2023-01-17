using User.Service.Data;
using User.Service.Services;
using Microsoft.EntityFrameworkCore;
using User.Service.Interceptors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(o => o.Interceptors.Add<ExceptionInterceptor>());
builder.Services.AddScoped<ITokenBuilder, TokenBuilder>();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.MapGrpcService<UserService>();

if (app.Environment.IsDevelopment())
    app.PopulateDb();

app.Run();