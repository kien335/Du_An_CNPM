using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;

namespace Dự_Án_CNPM.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var end = endDate ?? DateTime.Now.Date.AddDays(1).AddTicks(-1);

        // Lấy tất cả các phiên để tính toán thống kê toàn thời gian
        var allSessions = await _context.ParkingSessions.ToListAsync();

        // Thống kê hôm nay
        var todayStart = DateTime.Today;
        var todayEnd = todayStart.AddDays(1).AddTicks(-1);
        var todayRevenue = allSessions
            .Where(s => s.CheckOutTime >= todayStart && s.CheckOutTime <= todayEnd && s.TotalPrice.HasValue)
            .Sum(s => s.TotalPrice.Value);

        // Biểu đồ 7 ngày gần nhất
        var chartLabels = new List<string>();
        var chartData = new List<decimal>();
        for (int i = 6; i >= 0; i--)
        {
            var day = DateTime.Today.AddDays(-i);
            var ds = day;
            var de = day.AddDays(1).AddTicks(-1);
            var dayRevenue = allSessions
                .Where(s => s.CheckOutTime >= ds && s.CheckOutTime <= de && s.TotalPrice.HasValue)
                .Sum(s => s.TotalPrice.Value);
            chartLabels.Add(day.ToString("dd/MM"));
            chartData.Add(dayRevenue);
        }

        // Dữ liệu lọc cho bảng chi tiết
        var filteredSessions = allSessions
            .Where(s => s.CheckInTime >= start && s.CheckInTime <= end)
            .OrderByDescending(s => s.CheckInTime)
            .ToList();

        var viewModel = new ReportViewModel
        {
            StartDate = start,
            EndDate = end,
            Sessions = filteredSessions,
            TotalRevenue = allSessions.Where(s => s.TotalPrice.HasValue).Sum(s => s.TotalPrice.Value),
            TodayRevenue = todayRevenue,
            TotalSessions = allSessions.Count(s => s.Status == "Completed"),
            ActiveSessionsCount = allSessions.Count(s => s.Status == "Active"),
            ChartLabels = chartLabels,
            ChartData = chartData
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ExportExcel(DateTime startDate, DateTime endDate)
    {
        var endOfDay = endDate.Date.AddDays(1).AddTicks(-1);
        var sessions = await _context.ParkingSessions
            .Where(s => s.CheckInTime >= startDate && s.CheckInTime <= endOfDay)
            .OrderByDescending(s => s.CheckInTime)
            .ToListAsync();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Bao_cao_doanh_thu");
            
            // 1. Tiêu đề chính (Dòng 1)
            var titleRange = worksheet.Range(1, 1, 1, 8);
            titleRange.Merge().Value = "HỆ THỐNG QUẢN LÝ BÃI GIỮ XE - [NHÓM CỦA BẠN]";
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 16;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Font.FontColor = XLColor.White;
            titleRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1f497d");

            // 2. Tiêu đề phụ (Dòng 2)
            var subtitleRange = worksheet.Range(2, 1, 2, 8);
            subtitleRange.Merge().Value = "BÁO CÁO DOANH THU CHI TIẾT";
            subtitleRange.Style.Font.Bold = true;
            subtitleRange.Style.Font.FontSize = 14;
            subtitleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // 3. Thông tin lọc (Dòng 3)
            var infoRange = worksheet.Range(3, 1, 3, 8);
            infoRange.Merge().Value = $"Thời gian: từ {startDate:dd/MM/yyyy} đến {endDate:dd/MM/yyyy}";
            infoRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            infoRange.Style.Font.Italic = true;

            // 4. Header Bảng (Dòng 5)
            int headerRow = 5;
            worksheet.Cell(headerRow, 1).Value = "Mã lượt";
            worksheet.Cell(headerRow, 2).Value = "Mã thẻ";
            worksheet.Cell(headerRow, 3).Value = "Biển số";
            worksheet.Cell(headerRow, 4).Value = "Loại xe";
            worksheet.Cell(headerRow, 5).Value = "Giờ vào";
            worksheet.Cell(headerRow, 6).Value = "Giờ ra";
            worksheet.Cell(headerRow, 7).Value = "Thành tiền (VNĐ)";
            worksheet.Cell(headerRow, 8).Value = "Trạng thái";

            var tableHeader = worksheet.Range(headerRow, 1, headerRow, 8);
            tableHeader.Style.Font.Bold = true;
            tableHeader.Style.Fill.BackgroundColor = XLColor.LightBlue;
            tableHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableHeader.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // 5. Nội dung dữ liệu (Từ dòng 6)
            int currentRow = 6;
            decimal totalRevenue = 0;

            foreach (var session in sessions)
            {
                worksheet.Cell(currentRow, 1).Value = session.SessionId;
                worksheet.Cell(currentRow, 2).Value = session.CardId;
                worksheet.Cell(currentRow, 3).Value = session.LicensePlate;
                worksheet.Cell(currentRow, 4).Value = session.VehicleType;
                worksheet.Cell(currentRow, 5).Value = session.CheckInTime;
                worksheet.Cell(currentRow, 5).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                
                if (session.CheckOutTime.HasValue)
                {
                    worksheet.Cell(currentRow, 6).Value = session.CheckOutTime;
                    worksheet.Cell(currentRow, 6).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                }
                else
                {
                    worksheet.Cell(currentRow, 6).Value = "-";
                }

                decimal fee = session.TotalPrice ?? 0;
                worksheet.Cell(currentRow, 7).Value = fee;
                worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "#,##0";
                
                worksheet.Cell(currentRow, 8).Value = session.Status == "Completed" ? "Đã thanh toán" : "Đang gửi";
                
                // Kẻ bảng cho từng dòng
                worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                totalRevenue += fee;
                currentRow++;
            }

            // 6. Dòng tổng cộng (Cuối cùng)
            var totalRowRange = worksheet.Range(currentRow, 1, currentRow, 6);
            totalRowRange.Merge().Value = "TỔNG DOANH THU TRONG KỲ (VNĐ):";
            totalRowRange.Style.Font.Bold = true;
            totalRowRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            var totalValCell = worksheet.Cell(currentRow, 7);
            totalValCell.Value = totalRevenue;
            totalValCell.Style.Font.Bold = true;
            totalValCell.Style.NumberFormat.Format = "#,##0";
            totalValCell.Style.Font.FontColor = XLColor.Red;

            worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range(currentRow, 1, currentRow, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // 7. Hoàn thiện File
            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Bao-cao-doanh-thu-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }
    }
}
