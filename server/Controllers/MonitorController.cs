using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.Monitores;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        public MonitorController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] CreateMonitorModel createMonitorModel)
        {
            var monitor = new MonitorModel();
            monitor.NroInventario = createMonitorModel.NroInventario;
            monitor.Marca = createMonitorModel.Marca;
            monitor.Modelo = createMonitorModel.Modelo;
            monitor.Fuente = createMonitorModel.Fuente;
            monitor.Resolucion = createMonitorModel.Resolucion;
            monitor.OficinaId = createMonitorModel.oficinaId;
            monitor.NroSerie = createMonitorModel.NroSerie;
            _databaseService.Monitores.Add(monitor);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, monitor);
        }

        [HttpGet()]
        [Route("listado")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from m in _databaseService.Monitores
                join Oficinas in _databaseService.Oficinas
                on m.OficinaId equals Oficinas.OficinaId
                select new GetMonitores()
                {
                    NroInventario = m.NroInventario,
                    NroSerie = m.NroSerie,
                    Marca = m.Marca,
                    Modelo = m.Modelo,
                    Resolucion = m.Resolucion,
                    Fuente = m.Fuente,
                    Oficina = Oficinas.Nombre
                }
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}