using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace bangnaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        // GET: OrderFoods/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet("GetOrderFoodfrmDate")]
        public async Task<IActionResult> GetOrderFood(DateTime startData, DateTime endData)
        {
            // ปรับค่า startData และ endData ให้เป็นช่วงเวลาเที่ยงคืนถึง 23:59:59
            DateTime startDateOnly = startData.Date;
            DateTime endDateOnly = endData.Date;

            var orderFoodData = await (from orderFood in _context.OrderFoods
                                       join bedActive in _context.BedActives on orderFood.BedActiveId equals bedActive.Id
                                       join beds in _context.Beds on bedActive.BedId equals beds.Id
                                       join wards in _context.Wards on beds.WardId equals wards.Id
                                       join uc in _context.Ucs on bedActive.UdId equals uc.Id
                                       join foods in _context.Foods on orderFood.FoodId equals foods.Id
                                       where orderFood.Status == 1
                                       && orderFood.CreatedDate.HasValue
                                       && orderFood.CreatedDate.Value.Date >= startDateOnly
                                       && orderFood.CreatedDate.Value.Date <= endDateOnly
                                       select new
                                       {
                                           orderFood.Id,
                                           bedId = beds.Id,
                                           bedName = beds.Name,
                                           ward = wards.Id,
                                           wardName = wards.WardName,
                                           ucIc = uc.Id,
                                           ucName = uc.Name,
                                           patient = bedActive.HnName,
                                           foodId = foods.Id,
                                           foodName = foods.FoodName,
                                           CreatedDate = orderFood.CreatedDate.HasValue
                                           ? orderFood.CreatedDate.Value.ToString("dd-MM-yyyy HH:mm")
                                       : null

                                       }
                                       ).ToListAsync();

            return Ok(orderFoodData);
        }




        [HttpPost("AddOrderFood")]
        public async Task<IActionResult> AddOrderFood([FromBody] JsonDocument requestData)
        {
            if (requestData == null || !requestData.RootElement.TryGetProperty("data", out JsonElement dataElement))
            {
                return BadRequest("Data is required.");
            }

            // ดึงค่า CreatedBy
            int createdBy = requestData.RootElement.GetProperty("CreatedBy").GetInt32();

            // สร้างรายการเพื่อเก็บข้อมูลที่ตรงกับ Entity Model ของฐานข้อมูล
            var orderFoods = new List<OrderFood>();
            foreach (var item in dataElement.EnumerateArray())
            {
                var orderFood = new OrderFood
                {
                    BedActiveId = item.GetProperty("id").GetInt32(),
                    FoodId = item.GetProperty("foodType").GetInt32(),
                    Status = 1,
                    Remarks = item.GetProperty("note").GetString(),
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                };
                orderFoods.Add(orderFood);
            }

            await _context.OrderFoods.AddRangeAsync(orderFoods);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Data added successfully. {orderFoods.Count} items added." });
        }




        [HttpPost("UpdateOrderFood")]
        public async Task<IActionResult> UpdateOrderFood([Bind("Id,Status,UpdateBy")] OrderFood of)
        {
            if (of.Id == 0)
            {
                return BadRequest("Invalid OrderFood Id.");
            }

            var existingOrderFood = await _context.OrderFoods.FindAsync(of.Id);
            if (existingOrderFood == null)
            {
                return NotFound("OrderFoods not found");
            }
            existingOrderFood.Status = of.Status;
            existingOrderFood.UpdateBy = of.UpdateBy;
            existingOrderFood.LastUpdate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    message = "OrderFood updated successfully.",
                    orderFoodId = existingOrderFood.Id
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
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

    }
}
