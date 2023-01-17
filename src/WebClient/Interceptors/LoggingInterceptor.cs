using Grpc.Core;
using Grpc.Core.Interceptors;

namespace WebClient.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            var call = continuation(request, context);
            return new AsyncUnaryCall<TResponse>(call.ResponseAsync, call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    private void LogError(Exception ex)
    {
        _logger.LogError(ex, "Call error: {message}", ex.Message);
    }
}