using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
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
        private readonly BackEndForClinicAPIDBContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, string> _cd;


        public LoginController(IConfiguration configuration, BackEndForClinicAPIDBContext dbContext, IMemoryCache cache, ConcurrentDictionary<string, string> cd)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _cache = cache;
            _cd = cd;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> Login([FromRoute] Guid id)
        {
            UserModel user = await Authenticate(id);

            if (user != null)
            {
                Token token = new Token();
                token = _cache.Get<Token>(key: "UserIdForToken:" + id);
                var response = new Token();

                if (token == null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    var generatedToken = GenerateToken(user);

                    response = new Token()
                    {
                        TokenGenerated = (string)generatedToken,
                        ExpiryTime = DateTime.Now.AddMinutes(1),
                        RefreshToken = GenerateRefreshToken(id),
                        UserId = id
                    };

                    SetRefreshToken(response.RefreshToken);

                    _cache.Set("UserIdForToken:" + id, response, cacheEntryOptions);
                    return Ok(response);
                }
                return Ok(token);

                //SetRefreshToken(response.RefreshToken);

                //user.RefreshToken = refreshToken.Token;
                //user.TokenCreated = refreshToken.Created;
                //user.TokenExpires = refreshToken.Expires;
                //await _dbContext.SaveChangesAsync();

            }

            return NotFound("User with User ID:" + id + " not found.");
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult?> RefreshToken(Guid id)
        {

            UserModel user = await Authenticate(id);

            if (user != null)
            {
                var refreshToken = Request.Cookies["refreshToken"];

                //if (!user.RefreshToken.Equals(refreshToken))
                //{
                //    return Unauthorized("Invalid Refresh Token.");
                //}
                //else if (user.TokenExpires < DateTime.Now)
                //{
                //    return Unauthorized("Token expired.");
                //}

                string token = (string)GenerateToken(user);
                var newRefreshToken = GenerateRefreshToken(id);
                SetRefreshToken(newRefreshToken);
                
                return Ok(token);
            }

            return NotFound("Invalid Refresh Token");

        }


        [NonAction]
        public void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            
            Response.Cookies.Append("refreshToken",newRefreshToken.Token,cookieOptions);

        }


        [NonAction]
        private RefreshToken GenerateRefreshToken(Guid id)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
                UserId = id
            };

            return refreshToken;
        }


        [NonAction]
        private object GenerateToken(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
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


        [NonAction]
        private async Task<UserModel> Authenticate(Guid userId)
        {
            UserModel currentUser = await _dbContext.Doctors.FindAsync(userId);

            if (currentUser != null)
            {
                return currentUser;
            }
            else
            {
                currentUser = await _dbContext.Patients.FindAsync(userId);
                if (currentUser != null)
                {
                    return currentUser;

                }
                else
                {
                    currentUser = await _dbContext.Admins.FindAsync(userId);
                    if (currentUser != null)
                    {
                        return currentUser;

                    }
                }
            }

            return null;
        }
    }
}
