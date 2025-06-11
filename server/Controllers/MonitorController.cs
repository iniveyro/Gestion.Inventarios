using AutoMapper;
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
        private readonly IMapper _mapper;
        public MonitorController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] CreateMonitorModel createMonitorModel)
        {
            var monitor = _mapper.Map<MonitorModel>(createMonitorModel);
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
                select _mapper.Map<GetMonitores>(m)
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}