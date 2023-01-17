using i = GrpcTodoInternal;
using e = GrpcTodoExternal;

namespace Gateway.Service.Extensions;

public static class TodoMappingExtensions
{
    public static i.ListTodoReq MapToInternal(this e.ListTodoReq _, int userId)
    {
        return new i.ListTodoReq
        {
            UserId = userId,
        };
    }

    public static e.ListTodoRsp MapToExternal(this i.ListTodoRsp response)
    {
        var listTodoRsp = new e.ListTodoRsp();
        listTodoRsp.Entries.AddRange(response.Entries.Select(x => new e.TodoEntry
        {
            Id = x.Id,
            Title = x.Title,
            Text = x.Text,
            ColumnId = x.ColumnId,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt
        }));
        return listTodoRsp;
    }

    public static i.CreateTodoReq MapToInternal(this e.CreateTodoReq request, int userId)
    {
        return new i.CreateTodoReq
        {
            Title = request.Title,
            Text = request.Text,
            ColumnId = request.ColumnId,
            UserId = userId
        };
    }

    public static e.CreateTodoRsp MapToExternal(this i.CreateTodoRsp response)
    {
        return new e.CreateTodoRsp
        {
            Id = response.Id
        };
    }

    public static i.DeleteTodoReq MapToInternal(this e.DeleteTodoReq request, int userId)
    {
        return new i.DeleteTodoReq
        {
            Id = request.Id,
            UserId = userId
        };
    }

    public static e.DeleteTodoRsp MapToExternal(this i.DeleteTodoRsp response)
    {
        return new e.DeleteTodoRsp
        {
            Success = response.Success
        };
    }

    public static i.UpdateTodoReq MapToInternal(this e.UpdateTodoReq request, int userId)
    {
        return new i.UpdateTodoReq
        {
            Id = request.Id,
            Title = request.Title,
            Text = request.Text,
            ColumnId = request.ColumnId,
            UserId = userId
        };
    }

    public static e.UpdateTodoRsp MapToExternal(this i.UpdateTodoRsp response)
    {
        return new e.UpdateTodoRsp
        {
            Success = response.Success
        };
    }
}