using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dự_Án_CNPM.Models;

public class ParkingSession
{
    [Key]
    public int SessionId { get; set; }

    [Required]
    [StringLength(50)]
    public string CardId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string VehicleType { get; set; } = "Car"; // Default to Car

    [Required]
    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? CheckInImage { get; set; }

    public string? CheckOutImage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalPrice { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Completed

    public bool IsMonthlyTicket { get; set; } = false;

    [StringLength(10)]
    public string? SlotId { get; set; }
}
