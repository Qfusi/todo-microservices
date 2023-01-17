using Todo.Service.Models;

namespace Todo.Service.Data;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<TodoModel> GetAll(int userId) => _context.TodoEntries.Where(x => x.OwnerId == userId);

    public int Create(TodoModel model)
    {
        model.CreatedAt = DateTime.UtcNow;
        _context.Add(model);
        return SaveChanges() ? model.Id : -1;
    }

    public bool Delete(int id, int userId)
    {
        var entryToRemove = GetById(id, userId);
        if (entryToRemove == null)
            return false;

        _context.TodoEntries.Remove(entryToRemove);
        return SaveChanges();
    }

    public bool Update(TodoModel model)
    {
        if (_context.TodoEntries.Find(model.Id) is not TodoModel entryToUpdate)
            return false;

        entryToUpdate.Title = model.Title;
        entryToUpdate.Text = model.Text;
        entryToUpdate.Column = model.Column;
        entryToUpdate.UpdatedAt = model.UpdatedAt;

        return SaveChanges();
    }

    private TodoModel? GetById(int id, int userId) => _context.TodoEntries.FirstOrDefault(x => x.Id == id && x.OwnerId == userId);
    private bool SaveChanges() => _context.SaveChanges() >= 0;
}