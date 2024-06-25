using API.Data;
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
        public string Get([FromBody] Student user)
        {
            using(CRUDbContext context = new()){
                try
                {
                    var query = context.Students.Where(e => e.Email == user.Email).FirstOrDefault();
                    if (query != null && query.Password == user.Password)
                    {
                        var clains = new[]
                        {
                        new Claim(ClaimTypes.Name, user.Password),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
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
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception($"Cannot create token {ex}");
                }
            }

           
        }
    }
}
