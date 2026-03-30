using System.ComponentModel.DataAnnotations;

namespace Dự_Án_CNPM.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Staff"; // Admin hoặc Staff

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
