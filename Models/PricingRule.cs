using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dự_Án_CNPM.Models;

public class PricingRule
{
    [Key]
    public int RuleId { get; set; }

    [Required]
    [StringLength(50)]
    public string VehicleType { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }
}
