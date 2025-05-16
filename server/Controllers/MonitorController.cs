using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.User;

namespace server.Controllers
{
    [ApiController]
    [Route("api/v1/monitor")]
    public class MonitorController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        public MonitorController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost()]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateMonitorModel createMonitorModel)
        {
            var monitor = new MonitorModel();
            monitor.NroInventario = createMonitorModel.NroInventario;
            monitor.NroSerie = createMonitorModel.NroSerie;
            monitor.Marca = createMonitorModel.Marca;
            monitor.Modelo = createMonitorModel.Modelo;
            monitor.Fuente = createMonitorModel.Fuente;
            monitor.Resolucion = createMonitorModel.Resolucion;
            monitor.OficinaId = createMonitorModel.oficinaId;
            _databaseService.Monitores.Add(monitor);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, monitor);
        }

        [HttpPost]
        [Route("/api/[controller]/createuser")]
        [Authorize] //Solamente el admin
    public async Task<IActionResult> CrearUsuario([FromBody] CreateUserModel usr)
    {
        // Obtener el usuario actual desde los claims
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var currentUser = await _databaseService.Users.FindAsync(currentUserId);
        /*
        // Verificar si es admin
        if (currentUser == null || !currentUser.EsAdmin)
        {
            return Unauthorized("El usuario no es administrador");
        }
*/
        if (await _databaseService.Users.FirstOrDefaultAsync(u => u.Username == usr.Username) == null)
        {
            var usuario = new UserModel();
            usuario.NomApe = usr.NomApe;
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usr.Password);
            _databaseService.Users.Add(usuario);
            await _databaseService.SaveChangesAsync();
            return Ok(new { message = "Usuario creado correctamente" });    
        }
        return NotFound("Error, existe un usuario con ese correo");
    }

        [HttpGet()]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _databaseService.Monitores.ToArrayAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}