using Dự_Án_CNPM.Models;

namespace Dự_Án_CNPM.Models;

public class ReportViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TodayRevenue { get; set; }
    public int TotalSessions { get; set; }
    public int ActiveSessionsCount { get; set; }
    public List<string> ChartLabels { get; set; } = new();
    public List<decimal> ChartData { get; set; } = new();
    public List<ParkingSession> Sessions { get; set; } = new List<ParkingSession>();
}
