using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly IMapper _mapper;
        public EquipoController(DatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Route("obtener_equipo")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> GetByNroInvSerie([FromBody] DeleteEquipoModel equipoModel)
        {
            var data = await (
                from Equipos in _databaseService.Equipos
                where equipoModel.NroInventario == Equipos.NroInventario || equipoModel.NroSerie == Equipos.NroSerie
                select _mapper.Map<EquipoModel>(Equipos)
            ).FirstOrDefaultAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpDelete()]
        [Route("borrar")]
        [Authorize(Policy = "AuthenticatedUser")]
        public async Task<IActionResult> Delete([FromBody] DeleteEquipoModel deleteEquipoModel)
        {
            var data = await (
                from Equipos in _databaseService.Equipos
                where deleteEquipoModel.NroInventario == Equipos.NroInventario || deleteEquipoModel.NroSerie == Equipos.NroSerie
                select _mapper.Map<EquipoModel>(Equipos)
            ).FirstOrDefaultAsync();
            try
            {
                _databaseService.Equipos.Remove(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status202Accepted, $"Se elimino correctamente el Equipo con id {data.IdEquipo}");
        }
    }
}