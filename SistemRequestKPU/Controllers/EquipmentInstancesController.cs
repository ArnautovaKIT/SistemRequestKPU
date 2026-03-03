using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemRequestKPU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentInstancesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentInstancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var instances = await _context.EquipmentInstances
                .Include(ei => ei.EquipmentType)
                .Include(ei => ei.TechnicalObject)
                .Select(ei => new
                {
                    ei.Id,
                    Name = ei.EquipmentType.Name + " (" + (string.IsNullOrEmpty(ei.FactoryNumber) ? ei.InventoryNumber : ei.FactoryNumber) + ")",
                    ei.InventoryNumber,
                    ei.FactoryNumber,
                    ei.StationNumber,
                    ei.TechnicalNumber,
                    EquipmentTypeId = ei.EquipmentTypeId,
                    EquipmentTypeName = ei.EquipmentType.Name,
                    TechnicalObjectId = ei.TechnicalObjectId,
                    TechnicalObjectName = ei.TechnicalObject.Name,
                    ei.CurrentStatus
                })
                .ToListAsync();
            return Ok(instances);
        }

        [HttpGet("by-technical-object/{technicalObjectId}")]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetByTechnicalObject(int technicalObjectId)
        {
            var instances = await _context.EquipmentInstances
                .Include(ei => ei.EquipmentType)
                .Where(ei => ei.TechnicalObjectId == technicalObjectId)
                .Select(ei => new
                {
                    ei.Id,
                    Name = ei.EquipmentType.Name + " (" + (string.IsNullOrEmpty(ei.FactoryNumber) ? ei.InventoryNumber : ei.FactoryNumber) + ")",
                    ei.InventoryNumber,
                    ei.FactoryNumber
                })
                .ToListAsync();
            return Ok(instances);
        }

        [HttpGet("by-technological-unit/{technologicalUnitId}")]
        [Authorize(Roles = "Applicant,Dispatcher,Executor,Admin")]
        public async Task<IActionResult> GetByTechnologicalUnit(int technologicalUnitId)
        {
            var instances = await _context.EquipmentInstances
                .Include(ei => ei.EquipmentType)
                .Where(ei => ei.TechnologicalUnitId == technologicalUnitId)
                .Select(ei => new
                {
                    ei.Id,
                    Name = ei.EquipmentType.Name + " (" + (string.IsNullOrEmpty(ei.FactoryNumber) ? ei.InventoryNumber : ei.FactoryNumber) + ")",
                    ei.InventoryNumber,
                    ei.FactoryNumber
                })
                .ToListAsync();
            return Ok(instances);
        }
    }
}
