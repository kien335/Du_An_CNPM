using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dự_Án_CNPM.Controllers;

[Authorize]
public class ParkingSessionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParkingSessionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Danh sách các xe (Có bộ lọc tìm kiếm nâng cao)
    public async Task<IActionResult> Index(string? searchCardId, string? searchPlate, DateTime? fromDate, DateTime? toDate, string? status = "Active")
    {
        var pricingRules = await _context.PricingRules.ToListAsync();
        ViewBag.PricingRules = pricingRules;

        var query = _context.ParkingSessions.AsQueryable();

        if (!string.IsNullOrEmpty(searchCardId))
            query = query.Where(s => s.CardId.Contains(searchCardId));
        
        if (!string.IsNullOrEmpty(searchPlate))
            query = query.Where(s => s.LicensePlate.Contains(searchPlate));

        if (fromDate.HasValue)
            query = query.Where(s => s.CheckInTime >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(s => s.CheckInTime <= toDate.Value);

        if (!string.IsNullOrEmpty(status) && status != "All")
            query = query.Where(s => s.Status == status);

        var sessions = await query
            .OrderByDescending(s => s.CheckInTime)
            .ToListAsync();

        ViewBag.SearchCardId = searchCardId;
        ViewBag.SearchPlate = searchPlate;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
        ViewBag.CurrentStatus = status;

        return View(sessions);
    }

    [HttpGet]
    public async Task<IActionResult> CheckIn()
    {
        var availableSlots = await _context.ParkingSlots
            .Where(s => s.Status == "Empty")
            .OrderBy(s => s.SlotId)
            .ToListAsync();
        
        // Chỉ lấy các thẻ đang rảnh
        var availableCards = await _context.Cards
            .Where(c => c.Status == "Available")
            .ToListAsync();

        ViewBag.AvailableSlots = availableSlots;
        ViewBag.AvailableCards = availableCards;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(ParkingSession session)
    {
        // Kiểm tra trạng thái thẻ
        var card = await _context.Cards.FindAsync(session.CardId);
        if (card == null)
        {
            ModelState.AddModelError("CardId", "Thẻ không tồn tại trên hệ thống.");
        }
        else if (card.Status != "Available")
        {
            ModelState.AddModelError("CardId", $"Thẻ này hiện đang ở trạng thái: {card.Status}. Không thể Check-in.");
        }

        if (ModelState.IsValid && card != null)
        {
            // Kiểm tra vé tháng
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.LicensePlate == session.LicensePlate 
                                       && s.VehicleType == session.VehicleType 
                                       && s.IsActive 
                                       && s.EndDate >= DateTime.Now);

            if (subscription != null)
            {
                session.IsMonthlyTicket = true;
            }

            session.CheckInTime = DateTime.Now;
            session.Status = "Active";
            _context.Add(session);

            // Cập nhật trạng thái thẻ sang InUse
            card.Status = "InUse";
            _context.Update(card);

            // Tự động gán slot nếu để trống
            if (string.IsNullOrEmpty(session.SlotId))
            {
                var autoSlot = await _context.ParkingSlots
                    .FirstOrDefaultAsync(s => s.Status == "Empty");
                if (autoSlot != null)
                {
                    session.SlotId = autoSlot.SlotId;
                }
            }

            // Cập nhật trạng thái slot
            if (!string.IsNullOrEmpty(session.SlotId))
            {
                var slot = await _context.ParkingSlots.FindAsync(session.SlotId);
                if (slot != null)
                {
                    slot.Status = "Occupied";
                    _context.Update(slot);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Check-in thành công cho xe {session.LicensePlate}";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AvailableSlots = await _context.ParkingSlots.Where(s => s.Status == "Empty").ToListAsync();
        ViewBag.AvailableCards = await _context.Cards.Where(c => c.Status == "Available").ToListAsync();
        return View(session);
    }

    [HttpGet]
    public IActionResult CheckOut(string? id)
    {
        ViewBag.PreloadCardId = id;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveSession(string cardId)
    {
        var session = await _context.ParkingSessions
            .FirstOrDefaultAsync(s => s.CardId == cardId && s.Status == "Active");

        if (session == null)
            return Json(new { success = false, message = "Không tìm thấy phiên gửi xe đang hoạt động." });

        var pricingRule = await _context.PricingRules
            .FirstOrDefaultAsync(r => r.VehicleType == session.VehicleType);

        if (pricingRule == null)
            return Json(new { success = false, message = "Chưa cấu hình giá cho loại xe này." });

        var checkOutTime = DateTime.Now;
        var duration = checkOutTime - session.CheckInTime;
        var totalHours = (decimal)Math.Ceiling(duration.TotalHours);
        if (totalHours < 1) totalHours = 1;

        // Kiểm tra vé tháng một lần nữa lúc ra
        var isSubscriptionActive = await _context.Subscriptions
            .AnyAsync(s => s.LicensePlate == session.LicensePlate 
                        && s.VehicleType == session.VehicleType 
                        && s.IsActive 
                        && s.EndDate >= DateTime.Now);

        decimal fee = isSubscriptionActive ? 0 : (pricingRule.BasePrice + (totalHours * pricingRule.HourlyRate));

        return Json(new { 
            success = true,
            sessionId = session.SessionId,
            cardId = session.CardId,
            plate = session.LicensePlate,
            vehicleType = session.VehicleType,
            checkInTime = session.CheckInTime.ToString("dd/MM/yyyy HH:mm:ss"),
            checkInImage = session.CheckInImage,
            isMonthly = isSubscriptionActive,
            elapsedTime = $"{(int)duration.TotalHours}h {duration.Minutes}m",
            totalPrice = fee,
            totalPriceFormatted = fee.ToString("N0") + " VNĐ"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(string cardId, string? checkOutImage, decimal totalPrice, bool forceCheckOut = false)
    {
        var session = await _context.ParkingSessions
            .FirstOrDefaultAsync(s => s.CardId == cardId && s.Status == "Active");

        if (session == null)
        {
            TempData["Error"] = "Không tìm thấy lượt gửi xe cho thẻ này!";
            return RedirectToAction(nameof(Index));
        }

        session.CheckOutTime = DateTime.Now;
        session.TotalPrice = totalPrice;
        session.Status = "Completed";
        session.CheckOutImage = checkOutImage;

        // Giải phóng slot
        if (!string.IsNullOrEmpty(session.SlotId))
        {
            var slot = await _context.ParkingSlots.FindAsync(session.SlotId);
            if (slot != null)
            {
                slot.Status = "Empty";
                _context.Update(slot);
            }
        }

        // Cập nhật trạng thái thẻ về Available
        var card = await _context.Cards.FindAsync(cardId);
        if (card != null)
        {
            card.Status = "Available";
            _context.Update(card);
        }

        _context.Update(session);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Xe {session.LicensePlate} đã ra an toàn. Tổng tiền: {totalPrice:N0} VNĐ";
        return RedirectToAction(nameof(Index));
    }

    private decimal CalculateFee(DateTime checkIn, DateTime checkOut, PricingRule rule)
    {
        var duration = checkOut - checkIn;
        var totalHours = (decimal)Math.Ceiling(duration.TotalHours);
        if (totalHours < 1) totalHours = 1;

        return rule.BasePrice + (totalHours * rule.HourlyRate);
    }
}
