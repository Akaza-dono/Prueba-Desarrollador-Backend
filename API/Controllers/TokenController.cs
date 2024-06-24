using API.DDBBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private IConfiguration config;

        public TokenController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost("GenerarToken")]
        public string Get([FromBody]Usuario user)
        {
            var clains = new[]
            {
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Email, user.Correo),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Setting").GetSection("Apitoken").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                    claims: clains,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: cred
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;

        }
    }
}
