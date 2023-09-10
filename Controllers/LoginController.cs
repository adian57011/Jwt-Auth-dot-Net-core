using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task.DTOs;
using Task.EF;
using Task.EF.Models;

namespace Task.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly CoreDbContext db;
        private readonly IConfiguration config;
        public LoginController(CoreDbContext context, IConfiguration configuration)
        {
            db = context;
            config = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin obj)
        {
            var user = (from u in db.Users
                        where u.Username == obj.Username && u.Password == obj.Password
                        select u).SingleOrDefault();

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("Login Failed");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.NameIdentifier,user.Username),
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audienece"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
 
        }
    }
}
