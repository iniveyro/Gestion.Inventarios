using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Oficina;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OficinaController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public OficinaController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] CreateOficinaModel createOficinaModel)
        {
            var oficina = new OficinaModel();
            oficina.Nombre = createOficinaModel.Nombre;
            oficina.Ubicacion = createOficinaModel.Ubicacion;
            await _databaseService.Oficinas.AddAsync(oficina);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, oficina);
        }

        [HttpGet("listado")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _databaseService.Oficinas.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpDelete("borrar/{id}")]
        public async Task<IActionResult> Delete(int oficinaId)
        {
            var oficina = await _databaseService.Oficinas.FindAsync(oficinaId);
            _databaseService.Oficinas.Remove(oficina);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status200OK, "Oficina eliminada");
        }

    }
}