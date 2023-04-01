using AspNetAngular.Data;
using AspNetAngular.Entities;
using AspNetAngular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNetAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDb;

        public UserController(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            using (var trans = _appDb.Database.BeginTransaction())
            {
                var hasUserExists = _appDb.Users.Any(x => x.Username == request.Username);

                if (hasUserExists)
                {
                    return BadRequest();
                }

                var user = new ApplicationUser
                {
                    Username = request.Username,
                    Password = request.Password,
                    City = request.City,
                    ApplicationUserId = Guid.NewGuid(),
                };

                _appDb.Users.Add(user);
                _appDb.SaveChanges();
                trans.Commit();
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _appDb.Users
                .Where(x => x.Username == request.Username)
                .Where(x => x.Password == request.Password)
                .FirstOrDefault();

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("ApplicationUserId", user.ApplicationUserId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7183",
                audience: "https://localhost:7183",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")), SecurityAlgorithms.HmacSha256)
            );

            return Ok(new LoginResult
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }
    }
}
