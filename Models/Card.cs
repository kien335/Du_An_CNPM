using System.ComponentModel.DataAnnotations;

namespace Dự_Án_CNPM.Models;

public class Card
{
    [Key]
    [Required]
    [StringLength(50)]
    public string CardId { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Available"; // Available, InUse, Locked

    public string? Note { get; set; }
}
