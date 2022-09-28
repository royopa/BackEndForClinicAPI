using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    { 
        private readonly ConcurrentDictionary<string, string> _cd;
        private readonly IDoctorInterface _doctorInterface;


        public DoctorsController(ConcurrentDictionary<string, string> cd, IDoctorInterface doctorInterface)
        {
            _cd = cd;
            _doctorInterface = doctorInterface;
        }


        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctors()
        {
            return Ok(await _doctorInterface.GetDoctors()) ;
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<Doctor?>> GetDoctor([FromRoute] Guid id)
        {
            var doctor = await _doctorInterface.GetDoctor(id);

            if(doctor == null)
            {
                return NotFound("Doctor with ID: " + id + " was not found.");
            }
            return Ok(doctor);

        }

        
        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor(DoctorRequestModel addDoctorRequest)
        {
            var doctor = await _doctorInterface.AddDoctor(addDoctorRequest);

            if (doctor == null)
            {
                return NotFound("Error inserting Doctor record");
            }
            return Ok();


        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult<Doctor>> DeleteDoctor([FromRoute] Guid id)
        {
            var doctor = await _doctorInterface.DeleteDoctor(id);

            if (doctor == null)
            {
                return NotFound("Doctor with ID: " + id + " was not found.");
            }
            return Ok("Doctor Deleted.");
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, DoctorRequestModel updateDoctorRequest)
        {
            var doctor = await _doctorInterface.UpdateDoctor(id,updateDoctorRequest);

            if (doctor == null)
            {
                return NotFound("Doctor with ID: " + id + " was not found.");
            }
            return Ok("Doctor updated.");
        }


        [HttpGet("{id:guid}/slots")]
        [Authorize(Roles = "ADMIN,DOCTOR,PATIENT")]
        public IQueryable<AppointmentsDTO> GetDoctorsSchedule([FromRoute] Guid id)
        {
            if (User.IsInRole(nameof(Roles.PATIENT)))
            {
                return _doctorInterface.GetScheduleWithoutPatientInformation(id);
            }
            else
            {
                return _doctorInterface.GetSchedule(id);
            }
        }
    }
}
