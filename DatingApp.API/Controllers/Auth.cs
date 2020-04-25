using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        public IAuthRepository _Repository { get; }
        private readonly IConfiguration _config;
        public Auth(IAuthRepository repository, IConfiguration config)
        {
            _config = config;
            _Repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserDto dto)
        {
            dto.Username = dto.Username.ToLower();
            var user = new User
            {
                Username = dto.Username
            };

            if (await _Repository.Userexits(dto.Username) == false)
                return BadRequest("Username already exists");

            await _Repository.Register(user, dto.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]

        public async Task<IActionResult> login(UserloginDto dto)
        {
            var Existingrepo = await _Repository.Login(dto.username.ToLower(), dto.password);

            if (Existingrepo == null)
                return Unauthorized();

            var claims = new[]{
            new Claim(ClaimTypes.NameIdentifier,Existingrepo.Id.ToString()),
            new Claim(ClaimTypes.Name,Existingrepo.Username.ToString())

          };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var Creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var TokenDesc=new SecurityTokenDescriptor{
               Subject=new ClaimsIdentity(claims),
               Expires=DateTime.Now.AddDays(1),
               SigningCredentials=Creds
            };

            var tokenhandler=new JwtSecurityTokenHandler();

            var GeneratedToken=tokenhandler.CreateToken(TokenDesc);

            return Ok(new {
                    token= tokenhandler.WriteToken(GeneratedToken)
            });
        }
    }
}