using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Data;
using Dự_Án_CNPM.Models;

namespace Dự_Án_CNPM.Controllers
{
    public class PricingRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PricingRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PricingRules
        public async Task<IActionResult> Index()
        {
            return View(await _context.PricingRules.ToListAsync());
        }

        // GET: PricingRules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingRule = await _context.PricingRules
                .FirstOrDefaultAsync(m => m.RuleId == id);
            if (pricingRule == null)
            {
                return NotFound();
            }

            return View(pricingRule);
        }

        // GET: PricingRules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PricingRules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RuleId,VehicleType,HourlyRate,BasePrice")] PricingRule pricingRule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pricingRule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pricingRule);
        }

        // GET: PricingRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingRule = await _context.PricingRules.FindAsync(id);
            if (pricingRule == null)
            {
                return NotFound();
            }
            return View(pricingRule);
        }

        // POST: PricingRules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RuleId,VehicleType,HourlyRate,BasePrice")] PricingRule pricingRule)
        {
            if (id != pricingRule.RuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricingRule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingRuleExists(pricingRule.RuleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pricingRule);
        }

        // GET: PricingRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingRule = await _context.PricingRules
                .FirstOrDefaultAsync(m => m.RuleId == id);
            if (pricingRule == null)
            {
                return NotFound();
            }

            return View(pricingRule);
        }

        // POST: PricingRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pricingRule = await _context.PricingRules.FindAsync(id);
            if (pricingRule != null)
            {
                _context.PricingRules.Remove(pricingRule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricingRuleExists(int id)
        {
            return _context.PricingRules.Any(e => e.RuleId == id);
        }
    }
}
