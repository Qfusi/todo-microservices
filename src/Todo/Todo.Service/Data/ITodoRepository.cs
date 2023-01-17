using Todo.Service.Models;

namespace Todo.Service.Data;

public interface ITodoRepository
{
    IEnumerable<TodoModel> GetAll(int userId);
    int Create(TodoModel model);
    bool Delete(int id, int userId);
    bool Update(TodoModel model);
}