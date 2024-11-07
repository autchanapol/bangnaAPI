using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using System.Xml.Linq;


namespace bangnaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedsController : Controller
    {
        private readonly db_bangna1Context _context;

        public BedsController(db_bangna1Context context)
        {
            _context = context;
        }

        // GET: Beds
        public async Task<IActionResult> Index()
        {
            return _context.Beds != null ?
                        View(await _context.Beds.ToListAsync()) :
                        Problem("Entity set 'db_bangna1Context.Beds'  is null.");
        }


        // GET: Beds/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet("GetBeds")]
        public async Task<IActionResult> GetAll()
        {
            var bedData = await (from bed in _context.Beds
                                 join ward in _context.Wards on bed.WardId equals ward.Id
                                 where bed.Status == 1 // เงื่อนไขการกรอง status = 1
                                 select new
                                 {
                                     bed.Id,
                                     bed.Name,
                                     bed.Remarks,
                                     bed.WardId,
                                     bed.Actived,
                                     WardName = ward.WardName, // ดึงข้อมูล WardName จาก Ward
                                     bed.Status
                                 }).ToListAsync();

            return Ok(bedData);
        }


        [HttpGet("GetBedsFrmWard")]
        public async Task<IActionResult> GetBedsFrmWard(int wardId)
        {
            var bedData = await (from bed in _context.Beds
                                 join ward in _context.Wards on bed.WardId equals ward.Id
                                 where bed.Status == 1 && bed.WardId == wardId && (bed.Actived ?? 0) == 0
                                 select new
                                 {
                                     bed.Id,
                                     bed.Name
                                 }).ToListAsync();

            return Ok(bedData);
        }

        [HttpPost("InsertBed")]
        public async Task<IActionResult> InsertBed([FromBody] Bed bedDto)
        {
            if (bedDto == null)
            {
                return BadRequest("Invalid Bed data.");
            }

            // ตรวจสอบว่า WardId มีอยู่ในฐานข้อมูลหรือไม่
            var wardExists = await _context.Wards.AnyAsync(wd => wd.Id == bedDto.WardId);
            if (!wardExists)
            {
                return BadRequest("Invalid WardId. Ward does not exist.");
            }

            var newBed = new Bed
            {
                Name = bedDto.Name,
                WardId = bedDto.WardId,
                Remarks = bedDto.Remarks,
                Status = 1,
                Actived = 0,
                CreatedBy = bedDto.CreatedBy,
                CreatedDate = DateTime.Now
            };

            try
            {
                _context.Beds.Add(newBed);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Bed  inserted successfully.",
                    bedId = newBed.Id // ส่ง Ward.Id ที่ถูกสร้างกลับมา
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("UpdateBed")]
        public async Task<IActionResult> updateBed([Bind("Id,Name,WardId,Remarks,Status,UpdateBy")] Bed bed)
        {
            if (bed.Id == 0) // หรือใช้การตรวจสอบอื่นตามที่เหมาะสม
            {
                return BadRequest("Invalid Bed Id.");
            }

            var existingBed = await _context.Beds.FindAsync(bed.Id);
            if (existingBed == null)
            {
                return NotFound("Bed not found");
            }
            if (bed.WardId.HasValue)
            {
                // ตรวจสอบว่า WardId มีอยู่ในฐานข้อมูลหรือไม่
                var wardExists = await _context.Wards.AnyAsync(wd => wd.Id == bed.WardId);
                if (!wardExists)
                {
                    return BadRequest("Invalid WardId. Ward does not exist.");
                }
            }

            if (!string.IsNullOrEmpty(bed.Name))
            {
                existingBed.Name = bed.Name;
            }
            if (!string.IsNullOrEmpty(bed.Remarks))
            {
                existingBed.Remarks = bed.Remarks;
            }
            if (bed.WardId.HasValue)
            {
                existingBed.WardId = bed.WardId;
            }
            if (bed.Status.HasValue)
            {
                existingBed.Status = bed.Status;
            }
            existingBed.UpdateBy = bed.UpdateBy;
            existingBed.LastUpdate = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    message = "Bed updated successfully.",
                    bedId = existingBed.Id
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred during the update.");
            }


        }


    }
}
