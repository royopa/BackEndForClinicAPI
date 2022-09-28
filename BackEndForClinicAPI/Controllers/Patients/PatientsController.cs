using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Numerics;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private readonly IPatientInterface _patientInterface;


        public PatientsController(IPatientInterface patientInterface)
        {
            _patientInterface = patientInterface;
        }


        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient(PatientRequestModel addPatientRequest)
        {
            var patient = await _patientInterface.AddPatient(addPatientRequest);

            if (patient == null)
            {
                return NotFound("Error inserting Patient record");
            }
            return Ok(patient);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult<Patient>?> DeletePatient([FromRoute] Guid id)
        {
            var patient = await _patientInterface.DeletePatient(id);

            if (patient == null)
            {
                return NotFound("Patient with ID: " + id + " was not found.");
            }
            return Ok("Patient Deleted.");
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<Patient>?> GetPatient([FromRoute] Guid id)
        {
            var patient = await _patientInterface.GetPatient(id);

            if (patient == null)
            {
                return NotFound("Patient with ID: " + id + " was not found.");
            }
            return Ok(patient);
        }


        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return Ok(await _patientInterface.GetPatients());
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<Patient>?> UpdatePatient([FromRoute] Guid id, PatientRequestModel updatePatientRequest)
        {
            var patient = await _patientInterface.UpdatePatient(id, updatePatientRequest);

            if (patient == null)
            {
                return NotFound("Patient with ID: " + id + " was not found.");
            }
            return Ok("Patient updated.");
        }


        [HttpGet]
        [Route("appointments/{id:guid}")]
        public IQueryable<AppointmentsDTO> GetPreviousAppointments([FromRoute] Guid id)
        {
            return _patientInterface.GetPreviousAppointments(id);
        }
    }
}
