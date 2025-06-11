using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Aires;
using server.Models.DTOs.Equipo;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiresController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly IMapper _mapper;
        public AiresController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] CreateAAModel createAAModel)
        {
            var aire = _mapper.Map<AAModel>(createAAModel);
            _databaseService.Aires.Add(aire);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, aire);
        }

        [HttpGet()]
        [Route("listado")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from Aires in _databaseService.Aires
                join Oficinas in _databaseService.Oficinas
                on Aires.OficinaId equals Oficinas.OficinaId
                select _mapper.Map<GetAires>(Aires)
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

    }
}