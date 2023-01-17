using Microsoft.EntityFrameworkCore;
using Todo.Service.Data;
using Todo.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment())
    app.PopulateDb();

app.Run();