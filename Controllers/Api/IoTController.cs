using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;

namespace Dự_Án_CNPM.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class IoTController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IoTController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] IoTCheckInRequest request)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.LicensePlate == request.LicensePlate 
                                   && s.VehicleType == request.VehicleType 
                                   && s.IsActive 
                                   && s.EndDate >= DateTime.Now);

        var session = new ParkingSession
        {
            CardId = request.CardId,
            LicensePlate = request.LicensePlate,
            VehicleType = request.VehicleType,
            CheckInTime = DateTime.Now,
            CheckInImage = request.ImageUrl,
            Status = "Active",
            IsMonthlyTicket = subscription != null
        };

        _context.ParkingSessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Xe vào thành công", SessionId = session.SessionId });
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] IoTCheckOutRequest request)
    {
        var session = await _context.ParkingSessions
            .FirstOrDefaultAsync(s => s.CardId == request.CardId && s.Status == "Active");

        if (session == null)
        {
            return NotFound(new { Message = "Không tìm thấy lượt gửi xe" });
        }

        var checkOutTime = DateTime.Now;
        var pricingRule = await _context.PricingRules
            .FirstOrDefaultAsync(r => r.VehicleType == session.VehicleType);

        if (pricingRule == null)
        {
            return BadRequest(new { Message = "Không có quy tắc giá cho loại xe này" });
        }

        var duration = checkOutTime - session.CheckInTime;
        var totalHours = (decimal)Math.Ceiling(duration.TotalHours);
        if (totalHours < 1) totalHours = 1;

        session.CheckOutTime = checkOutTime;
        session.TotalPrice = pricingRule.BasePrice + (totalHours * pricingRule.HourlyRate);
        session.CheckOutImage = request.ImageUrl;
        session.Status = "Completed";

        _context.Update(session);
        await _context.SaveChangesAsync();

        return Ok(new { 
            Message = "Xe ra thành công", 
            TotalPrice = session.TotalPrice,
            LicensePlate = session.LicensePlate
        });
    }
}

public class IoTCheckInRequest
{
    public string CardId { get; set; } = "";
    public string LicensePlate { get; set; } = "";
    public string VehicleType { get; set; } = "Car";
    public string? ImageUrl { get; set; }
}

public class IoTCheckOutRequest
{
    public string CardId { get; set; } = "";
    public string? ImageUrl { get; set; }
}
