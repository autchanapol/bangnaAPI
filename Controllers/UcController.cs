using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;
using bangnaAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace bangnaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly db_bangna1Context _bangna1Context;

        // Constructor สำหรับ Inject db_bangna1Context
        public UcController(db_bangna1Context context)
        {
            _bangna1Context = context ?? throw new ArgumentNullException(nameof(context));
        }




        // GET: api/uc
        [HttpGet("GetUc")]
        public async Task<IActionResult> GetUc()
        {
            var ucs = await (from uc in _bangna1Context.Ucs 
                             where uc.Status == 1
                             select new { uc.Id,uc.Name } ).ToListAsync();
                             //Select(uc => new { uc.Id,uc.Name  }).ToArrayAsync();
            return Ok(ucs);
        }

        // GET: api/uc/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var uc = await _bangna1Context.Ucs.FindAsync(id);
            if (uc == null)
            {
                return NotFound();
            }
            return Ok(uc);
        }
    }
}
