using Gateway.Service.Services;
using static GrpcTodoInternal.Todo;
using static GrpcUser.User;

namespace Gateway.Registrations;

public static class RegisterGrpcServices
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();
        services.AddGrpcClient<TodoClient>(x =>
        {
            var address = configuration["TodoGrpcAddress"];
            x.Address = new Uri(address!);
        });
        services.AddGrpcClient<UserClient>(x =>
        {
            var address = configuration["UserGrpcAddress"];
            x.Address = new Uri(address!);
        });

        return services;
    }

    public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<TodoService>().EnableGrpcWeb();
        builder.MapGrpcService<UserService>().EnableGrpcWeb();
        return builder;
    }
}