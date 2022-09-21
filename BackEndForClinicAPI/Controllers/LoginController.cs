using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace BackEndForClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly BackEndForClinicAPIDBContext dbContext;

        public LoginController(IConfiguration configuration, BackEndForClinicAPIDBContext dbContext)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> Login([FromRoute] Guid id)
        {
            Doctor user = await Authenticate(id) as Doctor;

            if (user != null)
            {
                
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found.");
        }

        private object Generate(Doctor userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userModel.UserName),
                new Claim(ClaimTypes.Email, userModel.EmailAddress),
                new Claim(ClaimTypes.GivenName, userModel.GivenName),
                new Claim(ClaimTypes.Surname, userModel.Surname),
                new Claim(ClaimTypes.Role, userModel.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials : credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Doctor> Authenticate(Guid userId)
        {
            var currentUser = await dbContext.Doctors.FindAsync(userId);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
