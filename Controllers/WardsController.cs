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
    [ApiController]
    [Route("api/[controller]")]
    public class WardsController : Controller
    {
        private readonly db_bangna1Context _context;

        public WardsController(db_bangna1Context context)
        {
            _context = context;
        }

        // GET: Wards
        public async Task<IActionResult> Index()
        {
              return _context.Wards != null ? 
                          View(await _context.Wards.ToListAsync()) :
                          Problem("Entity set 'db_bangna1Context.Wards'  is null.");
        }

        // GET: api/uc
        [HttpGet("GetWards")]
        public async Task<IActionResult> GetAll()
        {
            var ucs = await _context.Wards
                .Where(wd => wd.Status == 1) // เงื่อนไขการกรอง status = 1
                .Select(wd => new { wd.Id, wd.WardName, wd.Remarks, wd.Status }).ToArrayAsync();
            return Ok(ucs);
        }

        // GET: Wards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wards == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            return View(ward);
        }

        // GET: Wards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WardName,Remarks,Status,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ward);
        }

        // GET: Wards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wards == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards.FindAsync(id);
            if (ward == null)
            {
                return NotFound();
            }
            return View(ward);
        }

        // POST: Wards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WardName,Remarks,Status,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] Ward ward)
        {
            if (id != ward.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WardExists(ward.Id))
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
            return View(ward);
        }

        // GET: Wards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wards == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return NotFound();
            }

            return View(ward);
        }

        // POST: Wards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wards == null)
            {
                return Problem("Entity set 'db_bangna1Context.Wards'  is null.");
            }
            var ward = await _context.Wards.FindAsync(id);
            if (ward != null)
            {
                _context.Wards.Remove(ward);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WardExists(int id)
        {
          return (_context.Wards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
