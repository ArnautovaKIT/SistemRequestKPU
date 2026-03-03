using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComplexesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplexesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var complexes = await _context.Complexes
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Type,
                    c.Location
                })
                .ToListAsync();
            return Ok(complexes);
        }
    }
}
