using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Componentes;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentesController : Controller
    {
        private readonly DatabaseService _databaseService;
        public ComponentesController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] createComponente c)
        {
            var data = new ComponenteModel()
            {
                Marca = c.Marca,
                Modelo = c.Modelo,
                Cantidad = c.Cantidad,
                Tipo = c.Tipo,
                Detalle = c.Detalle
            };
            _databaseService.Componentes.Add(data);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created,data);
        }

        [HttpGet()]
        [Route("listado")]
        public async Task<IActionResult> Listado()
        {
            var listado = await _databaseService.Componentes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listado);
        }
    }
}