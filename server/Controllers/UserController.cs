using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.User;

namespace server.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public UserController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserModel createUserModel)
        {
            var user = new UserModel();
            user.NomApe = createUserModel.NomApe;
            user.Password = createUserModel.Password;
            user.EsAdmin = true;
            user.Username = createUserModel.Username;
            await _databaseService.Users.AddAsync(user);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _databaseService.Users.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, data);
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int usuarioId)
        {
            var usuario = await _databaseService.Users.FindAsync(usuarioId);
            _databaseService.Users.Remove(usuario);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status200OK, "Usuario eliminado");
        }
    }
}