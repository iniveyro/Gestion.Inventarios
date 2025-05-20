using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.User;

namespace server.Controllers
{
    [ApiController]
    [Route("api/monitor")]
    public class MonitorController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        public MonitorController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("create")]
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
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _databaseService.Monitores.ToArrayAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}