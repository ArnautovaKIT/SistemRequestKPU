using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnicalObjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TechnicalObjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var objects = await _context.TechnicalObjects
                .Include(to => to.Complex)
                .Select(to => new
                {
                    to.Id,
                    Name = to.Name + " (" + to.ObjectType + ")",
                    to.ObjectType,
                    ComplexId = to.ComplexId,
                    ComplexName = to.Complex.Name
                })
                .ToListAsync();
            return Ok(objects);
        }
    }
}
