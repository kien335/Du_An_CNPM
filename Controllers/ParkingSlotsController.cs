using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;

namespace Dự_Án_CNPM.Controllers;

public class ParkingSlotsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParkingSlotsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ParkingSlots/Map
    public async Task<IActionResult> Map()
    {
        var slots = await _context.ParkingSlots.OrderBy(s => s.SlotId).ToListAsync();
        var activeSessions = await _context.ParkingSessions
            .Where(s => s.Status == "Active" && !string.IsNullOrEmpty(s.SlotId))
            .ToDictionaryAsync(s => s.SlotId!, s => s.LicensePlate);

        ViewBag.ActiveSessions = activeSessions;
        ViewBag.AvailableCount = slots.Count(s => s.Status == "Empty");
        
        return View(slots);
    }
}
