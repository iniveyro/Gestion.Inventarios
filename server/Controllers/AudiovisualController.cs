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
        public AudiovisualController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] CreateAudiovisualModel createAudiovisualModel)
        {
            var av = new AudiovisualModel();
            av.NroInventario = createAudiovisualModel.NroInventario;
            av.Marca = createAudiovisualModel.Marca;
            av.Modelo = createAudiovisualModel.Modelo;
            av.Accesorio = createAudiovisualModel.Accesorios;
            av.Tipo = createAudiovisualModel.Tipo;
            av.OficinaId = createAudiovisualModel.oficinaId;
            av.NroSerie = createAudiovisualModel.NroSerie;
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
                select new GetAudiovisuales()
                {
                    NroInventario = av.NroInventario,
                    NroSerie = av.NroSerie,
                    Marca = av.Marca,
                    Modelo = av.Modelo,
                    Accesorios = av.Accesorio,
                    Tipo = av.Tipo,
                    Oficina = Oficinas.Nombre
                }
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}