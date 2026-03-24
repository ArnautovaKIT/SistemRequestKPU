using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologicalUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TechnologicalUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/technologicalunits — все узлы
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var units = await _context.TechnologicalUnits
                .Select(tu => new
                {
                    tu.Id,
                    tu.Name,
                    tu.Code,
                    tu.Description,
                    WorkshopId = tu.WorkshopId,
                    WorkshopName = tu.Workshop.Name,
                    TechnicalObjectId = tu.TechnicalObjectId
                })
                .ToListAsync();

            return Ok(units);
        }

        // GET: api/technologicalunits/byobject?technicalObjectId=5
        [HttpGet("byobject")]
        public async Task<IActionResult> GetByTechnicalObject([FromQuery] int technicalObjectId)
        {
            var units = await _context.TechnologicalUnits
                .Where(tu => tu.TechnicalObjectId == technicalObjectId)
                .Select(tu => new
                {
                    tu.Id,
                    tu.Name,
                    tu.Code,
                    tu.Description,
                    WorkshopId = tu.WorkshopId,
                    WorkshopName = tu.Workshop.Name
                })
                .ToListAsync();

            return Ok(units);
        }

        // GET: api/technologicalunits/by-workshop/{workshopId}
        [HttpGet("by-workshop/{workshopId}")]
        public async Task<IActionResult> GetByWorkshop(int workshopId)
        {
            var units = await _context.TechnologicalUnits
                .Where(tu => tu.WorkshopId == workshopId)
                .Select(tu => new
                {
                    tu.Id,
                    tu.Name,
                    tu.Code,
                    tu.Description,
                    TechnicalObjectId = tu.TechnicalObjectId
                })
                .ToListAsync();

            return Ok(units);
        }
    }
}
