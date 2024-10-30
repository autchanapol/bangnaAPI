using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;

namespace bangnaAPI.Controllers
{
    public class BedActivesController : Controller
    {
        private readonly Data.db_bangna1Context _context;

        public BedActivesController(Data.db_bangna1Context context)
        {
            _context = context;
        }

        // GET: BedActives
        public async Task<IActionResult> Index()
        {
              return _context.BedActives != null ? 
                          View(await _context.BedActives.ToListAsync()) :
                          Problem("Entity set 'db_bangna1Context.BedActives'  is null.");
        }

        // GET: BedActives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BedActives == null)
            {
                return NotFound();
            }

            var bedActive = await _context.BedActives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bedActive == null)
            {
                return NotFound();
            }

            return View(bedActive);
        }

        // GET: BedActives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BedActives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BedId,UdId,HnId,HnName,Status,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] BedActive bedActive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedActive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bedActive);
        }

        // POST: BedActives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BedId,UdId,HnId,HnName,Status,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] BedActive bedActive)
        {
            if (id != bedActive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bedActive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedActiveExists(bedActive.Id))
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
            return View(bedActive);
        }



        // POST: BedActives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BedActives == null)
            {
                return Problem("Entity set 'db_bangna1Context.BedActives'  is null.");
            }
            var bedActive = await _context.BedActives.FindAsync(id);
            if (bedActive != null)
            {
                _context.BedActives.Remove(bedActive);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedActiveExists(int id)
        {
          return (_context.BedActives?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
