using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Audiovisuales;
using server.Models.DTOs.Equipo;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudiovisualController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly IMapper _mapper;
        public AudiovisualController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] CreateAudiovisualModel createAudiovisualModel)
        {
            var av = _mapper.Map<AudiovisualModel>(createAudiovisualModel);
            _databaseService.Audiovisuales.Add(av);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, av);
        }

        [HttpGet()]
        [Route("listado")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from av in _databaseService.Audiovisuales
                join Oficinas in _databaseService.Oficinas
                on av.OficinaId equals Oficinas.OficinaId
                select _mapper.Map<GetAudiovisuales>(av)
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}