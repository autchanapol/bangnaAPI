using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using bangnaAPI.ModelsTemp;

namespace bangnaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodsController : Controller
    {
        private readonly db_bangna1Context _context;

        public FoodsController(db_bangna1Context context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return _context.Foods != null ?
                        View(await _context.Foods.ToListAsync()) :
                                Problem("Entity set 'db_bangna1Context.Foods'  is null.");
        }

        [HttpGet("GetFoods")]
        public async Task<IActionResult> GetAll()
        {
            var foodData = await _context.Foods
                .Where(fd => fd.Status == 1)
                .Select(fd => new { fd.Id, fd.FoodName }).ToListAsync();
            return Ok(foodData);
        }




        private bool FoodExists(int id)
        {
            return (_context.Foods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
