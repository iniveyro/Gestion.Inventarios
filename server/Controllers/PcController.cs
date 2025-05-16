using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;

namespace server.Controllers
{
    [ApiController]
    [Route("api/v1/pc")]
    public class PcController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public PcController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreatePcModel createPcModel)
        {
            var pc = new PcModel();
            pc.NroInventario = createPcModel.NroInventario;
            pc.NroSerie = createPcModel.NroSerie;
            pc.Disco = createPcModel.Disco;
            pc.Marca = createPcModel.Marca;
            pc.Fuente = createPcModel.Fuente;
            pc.Procesador = createPcModel.Procesador;
            pc.Modelo = createPcModel.Modelo;
            pc.Ram = createPcModel.Ram;
            pc.TipoRam = createPcModel.TipoRam;
            pc.OficinaId = createPcModel.oficinaId; // <-- ¡asigná la oficina!
            _databaseService.Pcs.Add(pc);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, pc);
        }

        [HttpGet()]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _databaseService.Pcs.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

    }
}