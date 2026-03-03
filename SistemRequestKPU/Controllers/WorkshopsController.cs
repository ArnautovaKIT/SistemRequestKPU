using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkshopsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkshopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var workshops = await _context.Workshops
                .Select(w => new
                {
                    w.Id,
                    Name = w.Name + " (" + w.Code + ")",
                    w.Code,
                    ResponsiblePersonId = w.ResponsiblePersonId,
                    ResponsiblePersonName = w.ResponsiblePerson.Username
                })
                .ToListAsync();
            return Ok(workshops);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var workshop = await _context.Workshops
                .Where(w => w.Id == id)
                .Select(w => new
                {
                    w.Id,
                    Name = w.Name + " (" + w.Code + ")",
                    w.Code,
                    ResponsiblePersonId = w.ResponsiblePersonId,
                    ResponsiblePersonName = w.ResponsiblePerson.Username
                })
                .FirstOrDefaultAsync();

            if (workshop == null)
                return NotFound();

            return Ok(workshop);
        }
    }
}
