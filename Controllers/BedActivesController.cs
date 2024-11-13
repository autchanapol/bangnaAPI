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
    public class BedActivesController : Controller
    {
        private readonly db_bangna1Context _context;

        public BedActivesController(db_bangna1Context context)
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

        [HttpGet("GetBedsActive")]
        public async Task<IActionResult> GetBedsActive()
        {
            var bedActiveData = await (from bedActive in _context.BedActives
                                       join bed in _context.Beds on bedActive.BedId equals bed.Id
                                       join ward in _context.Wards on bed.WardId equals ward.Id
                                       join uc in _context.Ucs on bedActive.UdId equals uc.Id
                                       where bedActive.Status == 1
                                       select new
                                       {
                                           bedActive.Id,
                                           bedActive.HnId,
                                           bedActive.BedId,
                                           bedName = bed.Name,
                                           name = bedActive.HnName,
                                           bedActive.UdId,
                                           UdName = uc.Name,
                                           WardId = ward.Id,
                                           WardName = ward.WardName,
                                           CreatedDate = bedActive.CreatedDate.HasValue
                                       ? bedActive.CreatedDate.Value.ToString("dd-MM-yyyy HH:mm")
                                       : null
                                       }
                                        ).ToListAsync();

            return Ok(bedActiveData);
        }

        [HttpPost("InsertBedActive")]
        public async Task<IActionResult> InsertBedActive([FromBody] BedActive bedAcDto)
        {
            if (bedAcDto == null)
            {
                return BadRequest("Invalid BedActive data.");
            }

            var bedExists = await _context.Beds.AnyAsync(b => b.Id == bedAcDto.BedId);
            if (!bedExists)
            {
                return BadRequest("Invalid BedId. Bed does not exist.");
            }

            var newBedActive = new BedActive
            {
                BedId = bedAcDto.BedId,
                UdId = bedAcDto.UdId,
                HnId = "",
                HnName = bedAcDto.HnName,
                Remarks = bedAcDto.Remarks,
                Status = bedAcDto.Status,
                CreatedBy = bedAcDto.CreatedBy,
                CreatedDate = DateTime.Now
            };

            try
            {
                _context.BedActives.Add(newBedActive);
                await _context.SaveChangesAsync();

                var existingBed = await _context.Beds.FindAsync(bedAcDto.BedId);
                if (existingBed == null)
                {
                    return NotFound("Bed not found");
                }
                else
                {
                    existingBed.Actived = 1;
                    await _context.SaveChangesAsync(); // เพิ่มบรรทัดนี้เพื่อบันทึกการเปลี่ยนแปลง
                }


                return Ok(new
                {
                    success = true,
                    message = "BedActive inserted successfully.",
                    bedActiveId = newBedActive.Id // ส่ง Ward.Id ที่ถูกสร้างกลับมา
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        [HttpPost("UpdateBedActive")]
        public async Task<IActionResult> updateBedActive([Bind("Id,Status,UpdateBy")] BedActive bedActive)
        {
            if (bedActive.Id == 0)
            {
                return BadRequest("Invalid BedActive Id.");
            }
            var existingBedActive = await _context.BedActives.FindAsync(bedActive.Id);

            if (existingBedActive == null)
            {
                return NotFound("Bed not found");
            }
            existingBedActive.Status = bedActive.Status;
            existingBedActive.UpdateBy = bedActive.UpdateBy;

            try
            {

                // ค้นหา Bed โดยใช้ bed_id จาก existingBedActive
                var bed = await _context.Beds.FindAsync(existingBedActive.BedId);

                // ถ้าพบ Bed ที่เกี่ยวข้อง ให้ทำการอัปเดตฟิลด์ active เป็น 0
                if (bed != null)
                {
                    bed.Actived = 0;
                }

                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    message = "BedActive and Bed updated successfully.",
                    bedActiveId = existingBedActive.Id
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
        }


        [HttpGet("GetBedsActivefrmDate")]
        public async Task<IActionResult> GetBedsActive(DateTime dataOrder, int WardId)
        {
            var bedActiveData = await (from bedActive in _context.BedActives
                                       join bed in _context.Beds on bedActive.BedId equals bed.Id
                                       join ward in _context.Wards on bed.WardId equals ward.Id
                                       join uc in _context.Ucs on bedActive.UdId equals uc.Id
                                       where bedActive.Status == 1 && ward.Id == WardId
                                       && !_context.OrderFoods // NOT IN
                                       .Where(of => EF.Functions.DateDiffDay(of.CreatedDate, dataOrder) == 0 && of.Status == 1)
                                       .Select(of => of.BedActiveId)
                                       .Contains(bedActive.Id) // WHERE bedActive.Id
                                       select new
                                       {
                                           bedActive.Id,
                                           bedActive.BedId,
                                           bedName = bed.Name,
                                           bedActive.UdId,
                                           UdName = uc.Name,
                                           WardId = ward.Id,
                                           WardName = ward.WardName,
                                       }
                                        ).ToListAsync();

            return Ok(bedActiveData);
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
