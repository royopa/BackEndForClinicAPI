using System;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminInterface _adminInterface;


        public AdminController(IAdminInterface adminInterface)
        {
            _adminInterface = adminInterface;
        }

        [HttpPost]
        public async Task<ActionResult<Admin>> AddAdmin(AdminRequestModel addAdminRequest)
        {
            var admin = await _adminInterface.AddAdmin(addAdminRequest);

            if (admin == null)
            {
                return NotFound("Error inserting Admin record");
            }
            return Ok();
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<Admin>?> GetAdmin(Guid id)
        {
            var admin = await _adminInterface.GetAdmin(id);

            if (admin == null)
            {
                return NotFound("Admin with ID: " + id + " was not found.");
            }
            return Ok(admin);
        }


        [HttpGet]
        public async Task<ActionResult<List<Admin>>> GetAdmins()
        {
            return Ok(await _adminInterface.GetAdmins());
        }
    }
}

