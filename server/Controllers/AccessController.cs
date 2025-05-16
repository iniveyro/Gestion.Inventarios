using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models.DTOs.User;
using ApiAudiencia.Custom;
namespace ApiAudiencia.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AcessoController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly Utilidades _utilidades;
        public AcessoController(DatabaseService databaseService, Utilidades utilidades)
        {
            _databaseService = databaseService;
            _utilidades = utilidades;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Correo y contraseña son requeridos");
            } 
            var usuario = await _databaseService.Users
                .FirstOrDefaultAsync(x => x.Username == login.Username);
    
            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            
            bool credencialesValidas = false;
    
            if (usuario.Password.StartsWith("$2a$") || usuario.Password.StartsWith("$2b$")) 
            {
                credencialesValidas = BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password);
            }
            else
            {
                credencialesValidas = (usuario.Password == login.Password);
        
                if (credencialesValidas)
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(login.Password);
                    await _databaseService.SaveChangesAsync();
                }
            }

            if (!credencialesValidas)
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = _utilidades.generarToken(usuario);
    
            return Ok(new
            {
                token = token,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(60),
                usuario = new
                {
                    usuario.NomApe,
                    usuario.Password,
                    usuario.Rol,
                    usuario.UserId,
                    usuario.Username
                }
            });
        }
    }
}
