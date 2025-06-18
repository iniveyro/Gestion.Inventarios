using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.Pcs;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PcController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly IMapper _mapper;
        public PcController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Route("crear")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> Create([FromBody] CreatePcModel createPcModel)
        {
            var pc = _mapper.Map<PcModel>(createPcModel);
            _databaseService.Pcs.Add(pc);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, pc);
        }

        [HttpGet()]
        [Route("listado")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from pc in _databaseService.Pcs
                join Oficinas in _databaseService.Oficinas
                on pc.OficinaId equals Oficinas.OficinaId
                select _mapper.Map<GetPcs>(pc)
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}