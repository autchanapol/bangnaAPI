using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace bangnaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly db_bangna1Context _context;
        public UsersController(IConfiguration configuration, db_bangna1Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'db_bangna1Context.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Username,Password,Name,Role,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                // ตั้งค่า CreatedDate ให้เป็นเวลาปัจจุบันของเซิร์ฟเวอร์
                user.CreatedDate = DateTime.Now;

                // เพิ่ม user เข้าไปใน context
                _context.Add(user);

                // บันทึกการเปลี่ยนแปลง
                await _context.SaveChangesAsync();

                // ส่งกลับ JSON ที่บอกว่าการทำงานสำเร็จ
                return Ok(new
                {
                    success = true,
                    message = "User created successfully.",
                    userId = user.Id // หากคุณต้องการส่ง ID ของผู้ใช้ที่สร้างใหม่
                });
            }

            // ถ้ามีข้อผิดพลาด ส่งกลับ JSON พร้อมข้อมูลเกี่ยวกับข้อผิดพลาด
            return BadRequest(new
            {
                success = false,
                message = "Validation errors occurred.",
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Username and password are required."
                });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user != null)
            {
                // สร้าง JWT Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecretKey"]); // ดึง SecretKey จาก appsettings.json

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtConfig:TokenValidityMins"])),
                    Issuer = _configuration["JwtConfig:Issuer"],
                    Audience = _configuration["JwtConfig:Audience"],
                    SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // ส่ง Token กลับไปยัง Client

                return Ok(new
                {
                    success = true,
                    message = "Login successful.",
                    userId = user.Id,
                    name = user.Name,
                    role = user.Role,
                    token = tokenString
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Invalid username or password.",
                    userId = 0,
                    name = "",
                    role = 0
                });
            }
        }

        // โมเดลสำหรับการร้องขอข้อมูลการเข้าสู่ระบบ

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Name,Role,Status,CreatedDate,LastUpdate")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'db_bangna1Context.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
