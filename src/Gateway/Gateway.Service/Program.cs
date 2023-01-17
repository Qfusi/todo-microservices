using Gateway.Registrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration["JwtSigningKey"]!);
builder.Services.AddCors();
builder.Services.AddGrpcClients(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .WithOrigins("https://localhost:5000", "http://localhost:5040")
        .AllowCredentials());
}

app.UseJwtAuthentication();
app.UseGrpcWeb();
app.MapGrpcServices();

app.Run();