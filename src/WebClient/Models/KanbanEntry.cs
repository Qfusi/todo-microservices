using GrpcTodoExternal;

namespace WebClient.Models;

public class KanbanEntry
{
    public TodoEntry TodoEntry { get; init; }
    public bool DeleteToggled { get; set; }
    public bool EditToggled { get; set; }
    public string? EditedTitle { get; set; }
    public string? EditedText { get; set; }
    public int EditedColumn { get; set; }

    public KanbanEntry(TodoEntry todoEntry) => TodoEntry = todoEntry;
}