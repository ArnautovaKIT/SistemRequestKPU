using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnologicalUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TechnologicalUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var units = await _context.TechnologicalUnits
                .Include(tu => tu.Workshop)
                .Select(tu => new
                {
                    tu.Id,
                    Name = tu.Workshop.Name + " - " + tu.Name,
                    tu.Code,
                    tu.Description,
                    WorkshopId = tu.WorkshopId,
                    WorkshopName = tu.Workshop.Name
                })
                .ToListAsync();
            return Ok(units);
        }

        [HttpGet("by-workshop/{workshopId}")]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetByWorkshop(int workshopId)
        {
            var units = await _context.TechnologicalUnits
                .Where(tu => tu.WorkshopId == workshopId)
                .Select(tu => new
                {
                    tu.Id,
                    Name = tu.Name + " (" + tu.Code + ")",
                    tu.Code
                })
                .ToListAsync();
            return Ok(units);
        }
    }
}
