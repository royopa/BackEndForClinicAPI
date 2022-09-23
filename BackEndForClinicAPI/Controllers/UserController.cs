﻿using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BackEndForClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private AuthenticationAndAuthorizationHelpers _authenticationAndAuthorizationHelpers;
        //public UserController(AuthenticationAndAuthorizationHelpers authenticationAndAuthorizationHelpers)
        //{
        //    _authenticationAndAuthorizationHelpers = authenticationAndAuthorizationHelpers;
        //}

        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi welcome to public");
        }


        [HttpGet("Admins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }


        [HttpGet("Patient")]
        [Authorize(Roles = nameof(Roles.PATIENT))]
        public IActionResult PatientsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }


        [HttpGet("Doctor")]
        [Authorize(Roles = nameof(Roles.DOCTOR))]
        public IActionResult DoctorsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are a {currentUser.Role}");
        }


        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
