using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dự_Án_CNPM.Controllers;

[Authorize]
public class CardsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cards
    public async Task<IActionResult> Index()
    {
        return View(await _context.Cards.ToListAsync());
    }

    // GET: Cards/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cards/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CardId,Status,Note")] Card card)
    {
        if (ModelState.IsValid)
        {
            if (await _context.Cards.AnyAsync(c => c.CardId == card.CardId))
            {
                ModelState.AddModelError("CardId", "Mã thẻ này đã tồn tại trên hệ thống.");
                return View(card);
            }

            _context.Add(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(card);
    }

    // GET: Cards/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var card = await _context.Cards.FindAsync(id);
        if (card == null) return NotFound();
        return View(card);
    }

    // POST: Cards/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CardId,Status,Note")] Card card)
    {
        if (id != card.CardId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(card);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Cards.AnyAsync(c => c.CardId == card.CardId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(card);
    }

    // POST: Cards/Lock/5
    [HttpPost]
    public async Task<IActionResult> Lock(string id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card != null)
        {
            card.Status = "Locked";
            _context.Update(card);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Cards/Unlock/5
    [HttpPost]
    public async Task<IActionResult> Unlock(string id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card != null)
        {
            // If it was Locked, it becomes Available now. 
            // If it was InUse, locking it and unlocking it might need more care, 
            // but for now let's set it to its previous state or Available if unknown.
            card.Status = "Available"; 
            _context.Update(card);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Cards/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card != null)
        {
            if (card.Status == "InUse")
            {
                TempData["Error"] = "Không thể xóa thẻ đang trong quá trình sử dụng!";
                return RedirectToAction(nameof(Index));
            }
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
