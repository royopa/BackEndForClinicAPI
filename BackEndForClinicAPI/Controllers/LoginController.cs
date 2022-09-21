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
using System.Security.Cryptography;
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
            UserModel user = await Authenticate(id);

            if (user != null)
            {
                
                var token = GenerateToken(user);
                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken,user);
                return Ok(token);
            }

            return NotFound("User not found.");
        }

        [HttpPost("refreshtoken")]
        [Route("{id:guid}")]
        public async Task<IActionResult> RefreshToken([FromRoute] Guid id)
        {

            UserModel user = await Authenticate(id);

            if (user != null)
            {
                var refreshToken = Request.Cookies["refreshToken"];

                if (!user.RefreshToken.Equals(refreshToken))
                {
                    return Unauthorized("Invalid Refresh Token.");
                }
                else if (user.TokenExpires < DateTime.Now)
                {
                    return Unauthorized("Token expired.");
                }

                string token = (string)GenerateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken,user);

                return Ok(token);
            }
            return null;
            
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, UserModel user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            
            Response.Cookies.Append("refreshToken",newRefreshToken.Token,cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        private object GenerateToken(UserModel userModel)
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


        private async Task<UserModel> Authenticate(Guid userId)
        {
            UserModel currentUser = await dbContext.Doctors.FindAsync(userId);

            if (currentUser != null)
            {
                return currentUser;
            }
            else
            {
                currentUser = await dbContext.Patients.FindAsync(userId);
                if (currentUser != null)
                {
                    return currentUser;

                }
            }

            return null;
        }
    }
}
