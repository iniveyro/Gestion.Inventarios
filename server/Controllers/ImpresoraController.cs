using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.Impresoras;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImpresoraController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly IMapper _mapper;
        public ImpresoraController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }
        [HttpPost()]
        [Route("crear")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> Create([FromBody] CreateImpresoraModel createImpresora)
        {
            var impresora = _mapper.Map<ImpresoraModel>(createImpresora);
            _databaseService.Impresoras.Add(impresora);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, createImpresora);
        }

        [HttpGet()]
        [Route("listado")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from Impresoras in _databaseService.Impresoras
                join Oficinas in _databaseService.Oficinas
                on Impresoras.OficinaId equals Oficinas.OficinaId
                select _mapper.Map<GetImpresora>(Impresoras)
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}