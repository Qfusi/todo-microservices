namespace WebClient.Models;

public class KanbanSection
{
    public int Id { get; set; }
    public string Name { get; init; }
    public bool NewTaskOpen { get; set; }
    public string? NewTaskTitle { get; set; }
    public string? NewTaskText { get; set; }

    public KanbanSection(int id, string name)
    {
        Id = id;
        Name = name;
    }
}