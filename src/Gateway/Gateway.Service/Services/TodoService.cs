using System.Diagnostics;
using System.Security.Claims;
using Gateway.Service.Extensions;
using Grpc.Core;
using GrpcTodoExternal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using static GrpcTodoExternal.Todo;
using static GrpcTodoInternal.Todo;

namespace Gateway.Service.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TodoService : TodoBase
{
    private readonly ILogger<TodoService> _logger;
    private readonly TodoClient _todoClient;

    public TodoService(ILogger<TodoService> logger, TodoClient client)
    {
        _logger = logger;
        _todoClient = client;
    }

    public override async Task<ListTodoRsp> ListTodo(ListTodoReq request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = GetUserFromToken(httpContext);

        _logger.LogInformation("--> Routing ListTodo request");

        var response = await _todoClient.ListTodoAsync(request.MapToInternal(userId));

        _logger.LogInformation("--> Routing response to Client with {entries} entries", response.Entries.Count);

        return response.MapToExternal();
    }

    public override async Task<CreateTodoRsp> CreateTodo(CreateTodoReq request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = GetUserFromToken(httpContext);

        _logger.LogInformation("--> Routing CreateTodo request");

        var response = await _todoClient.CreateTodoAsync(request.MapToInternal(userId));

        _logger.LogInformation("--> Routing response '{id}' to Client", response.Id);

        return response.MapToExternal();
    }

    public override async Task<DeleteTodoRsp> DeleteTodo(DeleteTodoReq request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = GetUserFromToken(httpContext);

        _logger.LogInformation("--> Routing DeleteTodo request");

        var response = await _todoClient.DeleteTodoAsync(request.MapToInternal(userId));

        _logger.LogInformation("--> Routing response '{success}' to Client", response.Success);

        return response.MapToExternal();
    }

    public override async Task<UpdateTodoRsp> UpdateTodo(UpdateTodoReq request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = GetUserFromToken(httpContext);

        _logger.LogInformation("--> Routing UpdateTodo request");

        var response = await _todoClient.UpdateTodoAsync(request.MapToInternal(userId));

        _logger.LogInformation("--> Routing response '{success}' to Client", response.Success);

        return response.MapToExternal();
    }

    private static int GetUserFromToken(HttpContext context)
    {
        var userClaim = (context.User?.Identity as ClaimsIdentity)?.FindFirst(x => x?.Type == ClaimTypes.NameIdentifier);
        if (userClaim == null)
            throw new UnreachableException("Should not be possible to reach this since Authorization would not pass if user claim was missing. If you managed, well done!");
        if (!int.TryParse(userClaim.Value, out var parsedId))
            throw new UnreachableException("Should also not be possible since it would mean the token is inccoret, in which case, how did the request get past Authentication?");
        return parsedId;
    }
}
