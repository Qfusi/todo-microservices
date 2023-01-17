using System.ComponentModel.DataAnnotations;

namespace User.Service.Models;

public class UserModel
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}