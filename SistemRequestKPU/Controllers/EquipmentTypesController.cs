using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.EquipmentTypes
                .Select(et => new
                {
                    et.Id,
                    Name = et.Name + " (" + et.Model + ")",
                    et.Manufacturer,
                    et.Model,
                    et.Specifications
                })
                .ToListAsync();
            return Ok(types);
        }
    }
}
