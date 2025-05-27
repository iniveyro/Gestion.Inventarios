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
            user.Password = createUserModel.Password;
            user.EsAdmin = false;
            user.Username = createUserModel.Username;
            await _databaseService.Users.AddAsync(user);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpGet("listado")]
        public async Task<IActionResult> GetAll()
        {
            var user = await _databaseService.Users.FirstAsync();
            var data = new GetUser()
            {
                UserId = user.UserId,
                NomApe = user.NomApe,
                Username = user.Username,
                EsAdmin = user.EsAdmin,
            };
            return StatusCode(StatusCodes.Status200OK, data);
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