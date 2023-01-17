using Grpc.Core;
using GrpcTodoInternal;
using Todo.Service.Data;
using Todo.Service.Extensions;
using static GrpcTodoInternal.Todo;

namespace Todo.Service.Services;

public class TodoService : TodoBase
{
    private readonly ILogger<TodoService> _logger;
    private readonly ITodoRepository _todoRepository;

    public TodoService(ILogger<TodoService> logger, ITodoRepository todoRepository)
    {
        _logger = logger;
        _todoRepository = todoRepository;
    }

    public override Task<ListTodoRsp> ListTodo(ListTodoReq request, ServerCallContext context)
    {
        _logger.LogInformation("--> Fetching all Todo entries for user: {userId}", request.UserId);
        var todoList = _todoRepository.GetAll(request.UserId);

        return Task.FromResult(todoList.Map());
    }

    public override Task<CreateTodoRsp> CreateTodo(CreateTodoReq request, ServerCallContext context)
    {
        _logger.LogInformation($"--> Received create request");
        var id = _todoRepository.Create(request.Map());
        return Task.FromResult(new CreateTodoRsp { Id = id });
    }

    public override Task<DeleteTodoRsp> DeleteTodo(DeleteTodoReq request, ServerCallContext context)
    {
        _logger.LogInformation($"--> Received delete request");
        bool success = _todoRepository.Delete(request.Id, request.UserId);
        return Task.FromResult(new DeleteTodoRsp { Success = success });
    }

    public override Task<UpdateTodoRsp> UpdateTodo(UpdateTodoReq request, ServerCallContext context)
    {
        _logger.LogInformation($"--> Received update request");
        var success = _todoRepository.Update(request.Map());
        return Task.FromResult(new UpdateTodoRsp { Success = success });
    }
}