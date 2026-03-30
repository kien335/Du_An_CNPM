using System.ComponentModel.DataAnnotations;

namespace Dự_Án_CNPM.Models;

public class ParkingSlot
{
    [Key]
    [Required]
    [StringLength(10)]
    public string SlotId { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Empty"; // Empty hoặc Occupied
}
