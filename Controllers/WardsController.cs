using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace bangnaAPI.Controllers
{
    [Authorize]
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


        [HttpPost("InsertWard")]
        public async Task<IActionResult> InsertWard([FromBody] Ward wardDto)
        {
            if (wardDto == null)
            {
                return BadRequest("Invalid Ward data.");
            }

            // สร้าง object ของ Ward เพื่อแปลงข้อมูล
            var newWard = new Ward
            {
                WardName = wardDto.WardName,
                Remarks = wardDto.Remarks,
                Status = wardDto.Status ?? 1, // ถ้า Status ไม่ถูกส่งมาจะตั้งเป็น 1
                CreatedBy = wardDto.CreatedBy, // กำหนดค่าผู้สร้าง (กำหนดเองหรือรับจาก token/auth)
                CreatedDate = DateTime.Now, // วันที่สร้าง
            };

            // เพิ่มข้อมูลลงในตาราง Wards
           

            try
            {
                _context.Wards.Add(newWard);
                await _context.SaveChangesAsync(); // บันทึกข้อมูลลงฐานข้อมูล

                // ส่ง response กลับมาพร้อมกับ Ward.Id
                return Ok(new
                {
                    success = true,
                    message = "Ward inserted successfully.",
                    wardId = newWard.Id // ส่ง Ward.Id ที่ถูกสร้างกลับมา
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateWard")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateWard([Bind("Id,WardName,Remarks,Status,CreatedBy,CreatedDate,UpdateBy,LastUpdate")] Ward ward)
        {
            // ตรวจสอบว่ามีการส่งค่า Id มาหรือไม่
            if (ward.Id == 0) // หรือใช้การตรวจสอบอื่นตามที่เหมาะสม
            {
                return BadRequest("Invalid Ward Id.");
            }

            // ดึงข้อมูล Ward เดิมจากฐานข้อมูล
            var existingWard = await _context.Wards.FindAsync(ward.Id);

            if (existingWard == null)
            {
                return NotFound("Ward not found");
            }

            // อัปเดตเฉพาะฟิลด์ที่ส่งมา
            if (!string.IsNullOrEmpty(ward.WardName))
            {
                existingWard.WardName = ward.WardName;
            }

            if (!string.IsNullOrEmpty(ward.Remarks))
            {
                existingWard.Remarks = ward.Remarks;
            }

            if (ward.Status.HasValue)
            {
                existingWard.Status = ward.Status;
            }

            existingWard.UpdateBy = ward.UpdateBy;
            existingWard.LastUpdate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    message = "Ward updated successfully.",
                    wardId = existingWard.Id
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
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
