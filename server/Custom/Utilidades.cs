using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.Models;

namespace ApiAudiencia.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string generarToken(UserModel modelo)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _configuration["JWT:Key"];
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, modelo.UserId.ToString()),
               new Claim(ClaimTypes.Name, modelo.Username!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //crear detalle del token con los claims, tiempo de expiración y credenciales
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(60), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
