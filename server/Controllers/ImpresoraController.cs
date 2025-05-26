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
        public ImpresoraController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        [HttpPost()]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateImpresoraModel createImpresora)
        {
            var impresora = new ImpresoraModel();
            impresora.NroInventario = createImpresora.NroInventario;
            impresora.Marca = createImpresora.Marca;
            impresora.Modelo = createImpresora.Modelo;
            impresora.OficinaId = createImpresora.oficinaId;
            impresora.NroSerie = createImpresora.NroSerie;
            impresora.Consumible = createImpresora.Consumible;
            impresora.TonnerModelo = createImpresora.TonnerModelo;
            impresora.Tipo = createImpresora.Tipo;
            _databaseService.Impresoras.Add(impresora);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, createImpresora);
        }

        [HttpGet()]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from Impresoras in _databaseService.Impresoras
                join Oficinas in _databaseService.Oficinas
                on Impresoras.OficinaId equals Oficinas.OficinaId
                select new GetImpresora()
                {
                    NroInventario = Impresoras.NroInventario,
                    NroSerie = Impresoras.NroSerie,
                    TonnerModelo = Impresoras.TonnerModelo,
                    Tipo = Impresoras.Tipo,
                    Consumible = Impresoras.Consumible,
                    Marca = Impresoras.Marca,
                    Modelo = Impresoras.Modelo,
                    Oficina = Oficinas.Nombre
                }
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}