using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.Pcs;

namespace server.Controllers
{
    [ApiController]
    [Route("api/pc")]
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
            pc.Disco = createPcModel.Disco;
            pc.Marca = createPcModel.Marca;
            pc.Fuente = createPcModel.Fuente;
            pc.Procesador = createPcModel.Procesador;
            pc.Modelo = createPcModel.Modelo;
            pc.Ram = createPcModel.Ram;
            pc.TipoRam = createPcModel.TipoRam;
            pc.OficinaId = createPcModel.oficinaId;
            pc.NroSerie = createPcModel.NroSerie;
            _databaseService.Pcs.Add(pc);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, pc);
        }

        [HttpGet()]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from pc in _databaseService.Pcs
                join Oficinas in _databaseService.Oficinas
                on pc.OficinaId equals Oficinas.OficinaId
                select new GetPcs()
                {
                    NroInventario = pc.NroInventario,
                    NroSerie = pc.NroSerie,
                    Marca = pc.Marca,
                    Modelo = pc.Modelo,
                    Procesador = pc.Procesador,
                    Ram = pc.Ram,
                    TipoRam = pc.TipoRam,
                    Disco = pc.Disco,
                    Fuente = pc.Fuente,
                    Oficina = Oficinas.Nombre
                }
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

    }
}