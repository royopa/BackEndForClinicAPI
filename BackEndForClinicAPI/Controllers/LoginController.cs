﻿using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEndForClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found.");
        }

        private object Generate(UserModel userModel)
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

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstants.Users.FirstOrDefault( o => o.UserName.ToLower() == userLogin.UserName.ToLower() && o.Password == userLogin.Password);
            
            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
