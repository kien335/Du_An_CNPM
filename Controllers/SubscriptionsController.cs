using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dự_Án_CNPM.Controllers;

[Authorize]
public class SubscriptionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SubscriptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Subscriptions
    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var subscriptions = from s in _context.Subscriptions
                           select s;

        if (!string.IsNullOrEmpty(searchString))
        {
            subscriptions = subscriptions.Where(s => s.LicensePlate.Contains(searchString) || s.CustomerName.Contains(searchString));
        }

        return View(await subscriptions.OrderByDescending(s => s.EndDate).ToListAsync());
    }

    // GET: Subscriptions/Create
    public IActionResult Create()
    {
        return View(new Subscription { StartDate = DateTime.Today, EndDate = DateTime.Today.AddMonths(1) });
    }

    // POST: Subscriptions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Subscription subscription)
    {
        if (ModelState.IsValid)
        {
            _context.Add(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subscription);
    }

    // GET: Subscriptions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // POST: Subscriptions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Subscription subscription)
    {
        if (id != subscription.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(subscription.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(subscription);
    }

    // GET: Subscriptions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subscription == null) return NotFound();

        return View(subscription);
    }

    // POST: Subscriptions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription != null)
        {
            _context.Subscriptions.Remove(subscription);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubscriptionExists(int id)
    {
        return _context.Subscriptions.Any(e => e.Id == id);
    }
}
