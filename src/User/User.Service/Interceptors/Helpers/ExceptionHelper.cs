using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace User.Service.Interceptors.Helpers;

public static class ExceptionHelpers
{
    public static RpcException Handle<T>(this Exception exception, ILogger<T> logger, Guid correlationId) =>
        exception switch
        {
            TimeoutException => HandleTimeoutException((TimeoutException)exception, logger, correlationId),
            SqlException => HandleSqlException((SqlException)exception, logger, correlationId),
            RpcException => HandleRpcException((RpcException)exception, logger, correlationId),
            _ => HandleDefault(exception, logger, correlationId)
        };

    private static RpcException HandleTimeoutException<T>(TimeoutException exception, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {correlationId} - A timeout occurred", correlationId);

        var status = new Status(StatusCode.Internal, "An external resource did not answer within the time limit");

        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static RpcException HandleSqlException<T>(SqlException exception, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {correlationId} - An SQL error occurred", correlationId);
        Status status;

        if (exception.Number == -2)
        {
            status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
        }
        else
        {
            status = new Status(StatusCode.Internal, "SQL error");
        }
        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static RpcException HandleRpcException<T>(RpcException exception, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {correlationId} - An error occurred", correlationId);
        var trailers = exception.Trailers;
        trailers.Add(CreateTrailers(correlationId)[0]);
        return new RpcException(new Status(exception.StatusCode, exception.Message), trailers);
    }

    private static RpcException HandleDefault<T>(Exception exception, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {correlationId} - An error occurred", correlationId);
        return new RpcException(new Status(StatusCode.Internal, exception.Message), CreateTrailers(correlationId));
    }

    /// <summary>
    ///  Adding the correlation to Response Trailers
    /// </summary>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    private static Metadata CreateTrailers(Guid correlationId)
    {
        return new Metadata
        {
            { "CorrelationId", correlationId.ToString() }
        };
    }
}