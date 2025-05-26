using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Aires;
using server.Models.DTOs.Equipo;

namespace server.Controllers
{
    [ApiController]
    [Route("api/aires")]
    public class AAController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public AAController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateAAModel createAAModel)
        {
            var aire = new AAModel();
            aire.NroInventario = createAAModel.NroInventario;
            aire.Marca = createAAModel.Marca;
            aire.Modelo = createAAModel.Modelo;
            aire.Potencia = createAAModel.Potencia;
            aire.Frigorias = createAAModel.Frigorias;
            aire.Tipo = createAAModel.Tipo;
            aire.OficinaId = createAAModel.oficinaId;
            aire.NroSerie = createAAModel.NroSerie;
            _databaseService.Aires.Add(aire);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, aire);
        }

        [HttpGet()]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await (
                from Aires in _databaseService.Aires
                join Oficinas in _databaseService.Oficinas
                on Aires.OficinaId equals Oficinas.OficinaId
                select new GetAires()
                {
                    NroInventario = Aires.NroInventario,
                    NroSerie = Aires.NroSerie,
                    Marca = Aires.Marca,
                    Modelo = Aires.Modelo,
                    Frigorias = Aires.Frigorias,
                    Tipo = Aires.Tipo,
                    Potencia = Aires.Potencia,
                    Oficina = Oficinas.Nombre
                }
                ).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

    }
}