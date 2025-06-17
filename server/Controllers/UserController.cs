using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.User;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        public UserController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] CreateUserModel createUserModel)
        {
            var user = new UserModel();
            user.NomApe = createUserModel.NomApe;
            user.Password = BCrypt.Net.BCrypt.HashPassword(createUserModel.Password);
            user.EsAdmin = false;
            user.Username = createUserModel.Username;
            await _databaseService.Users.AddAsync(user);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpGet("listado")]
        public async Task<IActionResult> GetAll()
        {
            var listado = await _databaseService.Users.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listado);
        }

        [HttpPut("actualizarpass")]
        [Authorize]
        public async Task<IActionResult> ActualizarPassword(UpdatePassword updatePassword)
        {
            if (updatePassword.NuevaContra == updatePassword.ConfirmarContra)
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var usuario = await _databaseService.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == id);
                try
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(updatePassword.NuevaContra);
                    await _databaseService.SaveAsync();
                    return StatusCode(StatusCodes.Status200OK, "Se modifico la contraseña correctamente");
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el usuario");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Las contraseñas no coinciden");
            }
        }

        [HttpDelete("borrar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _databaseService.Users.FindAsync(id);
            _databaseService.Users.Remove(usuario);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status200OK, "Usuario eliminado");
        }
    }
}