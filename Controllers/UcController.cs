using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using bangnaAPI.Data;

namespace bangnaAPI.Controllers
{
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
        public async Task<IActionResult> GetAll()
        {
            var ucs = await _bangna1Context.Ucs.Select(uc => new { uc.Id,uc.Name,uc.Remarks,uc.Status   }).ToArrayAsync();
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
