using GrpcTodoInternal;
using Todo.Service.Models;
using Google.Protobuf.WellKnownTypes;

namespace Todo.Service.Extensions;

public static class TodoMappingExtensions
{
    public static TodoModel Map(this CreateTodoReq request)
    {
        return new TodoModel
        {
            Title = request.Title,
            Text = request.Text,
            Column = request.ColumnId,
            OwnerId = request.UserId
        };
    }

    public static ListTodoRsp Map(this IEnumerable<TodoModel> listedModels)
    {
        var listTodoRsp = new ListTodoRsp();
        listTodoRsp.Entries.AddRange(listedModels.Select(x => new TodoEntry
        {
            Id = x.Id,
            Title = x.Title,
            Text = x.Text,
            ColumnId = x.Column,
            CreatedAt = Timestamp.FromDateTime(x.CreatedAt),
            UpdatedAt = x.UpdatedAt != null ? Timestamp.FromDateTime(x.UpdatedAt ?? new DateTime()) : null
        }));
        return listTodoRsp;
    }

    public static TodoModel Map(this UpdateTodoReq request)
    {
        return new TodoModel
        {
            Id = request.Id,
            Title = request.Title,
            Text = request.Text,
            Column = request.ColumnId,
            UpdatedAt = DateTime.UtcNow,
            OwnerId = request.UserId
        };
    }
}