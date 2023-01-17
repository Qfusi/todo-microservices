using System.ComponentModel.DataAnnotations;

namespace Todo.Service.Models;

public class TodoModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    public string? Text { get; set; }

    [Required]
    public int Column { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public int OwnerId { get; set; }
}