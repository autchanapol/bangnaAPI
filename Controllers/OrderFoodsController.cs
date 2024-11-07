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
    public class OrderFoodsController : Controller
    {
        private readonly db_bangna1Context _context;

        public OrderFoodsController(db_bangna1Context context)
        {
            _context = context;
        }

        // GET: OrderFoods
        public async Task<IActionResult> Index()
        {
              return _context.OrderFoods != null ? 
                          View(await _context.OrderFoods.ToListAsync()) :
                          Problem("Entity set 'db_bangna1Context.OrderFoods'  is null.");
        }

        // GET: OrderFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderFoods == null)
            {
                return NotFound();
            }

            var orderFood = await _context.OrderFoods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderFood == null)
            {
                return NotFound();
            }

            return View(orderFood);
        }

        // GET: OrderFoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderFoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BedActiveId,FoodId,Status,Remarks,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] OrderFood orderFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderFood);
        }

        // GET: OrderFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderFoods == null)
            {
                return NotFound();
            }

            var orderFood = await _context.OrderFoods.FindAsync(id);
            if (orderFood == null)
            {
                return NotFound();
            }
            return View(orderFood);
        }

        // POST: OrderFoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BedActiveId,FoodId,Status,Remarks,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] OrderFood orderFood)
        {
            if (id != orderFood.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderFoodExists(orderFood.Id))
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
            return View(orderFood);
        }

        // GET: OrderFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderFoods == null)
            {
                return NotFound();
            }

            var orderFood = await _context.OrderFoods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderFood == null)
            {
                return NotFound();
            }

            return View(orderFood);
        }

        // POST: OrderFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderFoods == null)
            {
                return Problem("Entity set 'db_bangna1Context.OrderFoods'  is null.");
            }
            var orderFood = await _context.OrderFoods.FindAsync(id);
            if (orderFood != null)
            {
                _context.OrderFoods.Remove(orderFood);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderFoodExists(int id)
        {
          return (_context.OrderFoods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
