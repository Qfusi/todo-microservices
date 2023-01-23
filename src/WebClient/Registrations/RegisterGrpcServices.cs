using Blazored.LocalStorage;
using Grpc.Net.Client.Web;
using WebClient.Constants;
using WebClient.Interceptors;
using static GrpcTodoExternal.Todo;
using static GrpcUser.User;

namespace WebClient.Registrations;

public static class RegisterGrpcServices
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<LoggingInterceptor>();
        services.AddGrpcClient<TodoClient>(o =>
        {
            var address = configuration["GatewayGrpcAddress"];
            o.Address = new Uri(address!);
            o.ChannelOptionsActions.Add(opt => opt.UnsafeUseInsecureChannelCallCredentials = true);
        }).AddInterceptor<LoggingInterceptor>()
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
        }).AddCallCredentials(async (context, metadata, IServiceProvider) =>
        {
            var localStorage = IServiceProvider.GetService<ILocalStorageService>();
            var token = await localStorage!.GetItemAsStringAsync(LocalStorageConstants.TokenKey);
            metadata.Add("Authorization", $"Bearer {token}");
        });

        services.AddGrpcClient<UserClient>(o =>
        {
            var address = configuration["GatewayGrpcAddress"];
            o.Address = new Uri(address!);
            o.ChannelOptionsActions.Add(opt => opt.UnsafeUseInsecureChannelCallCredentials = true);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
        }).AddCallCredentials(async (context, metadata, IServiceProvider) =>
        {
            var localStorage = IServiceProvider.GetService<ILocalStorageService>();
            var token = await localStorage!.GetItemAsStringAsync(LocalStorageConstants.TokenKey);
            metadata.Add("Authorization", $"Bearer {token}");
        });

        return services;
    }
}