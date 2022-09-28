using System.Data;
using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentInterface _appointmentInterface;

        public AppointmentsController(IAppointmentInterface appointmentInterface)
        {
            _appointmentInterface = appointmentInterface;
        }


        [HttpPost]
        [Authorize(Roles = nameof(Roles.PATIENT))]
        public async Task<ActionResult<Appointment>> AddAppointment(AppointmentRequestModel addAppointmentRequest)
        {
            var appointment = await _appointmentInterface.AddAppointment(addAppointmentRequest);

            if (appointment == null)
            {
                return NotFound("The Doctor (Doctor ID: "+addAppointmentRequest.DoctorId.ToString()+") has no free slots on the given date or the number of slots are not sufficient enough.");
            }
            return Ok(appointment);

        }


        //below end point is used to get appointment details by passing the appointment id
        //[HttpGet("/appointment-details/{id:guid}")]
        [HttpGet]
        [Authorize(Roles = "DOCTOR,PATIENT")]
        [Route("details/{id:guid}")]
        public IQueryable<AppointmentsDTO> GetAppointmentDetails([FromRoute] Guid id)
        {
            return _appointmentInterface.GetAppointmentDetails(id);
        }



        [HttpDelete]
        [Authorize(Roles = "DOCTOR,ADMIN")]
        [Route("cancel/{id:guid}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment([FromRoute] Guid id)
        {
            var appointment = await _appointmentInterface.CancelAppointment(id);

            if (appointment == null)
            {
                return NotFound("Appointment with ID: " + id + " was not found.");
            }
            return Ok("Appointment with ID: " + id + " was cancelled.");
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("most-appointments/{date:DateTime}")]
        public async Task<ActionResult<List<Appointment>>> GetMostNumberOfAppointments([FromRoute] DateTime date)
        {
            return Ok( await _appointmentInterface.GetMostNumberOfAppointments(date));
        }


        [HttpGet]
        [Route("all-appointments")]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
        {
            return Ok( await _appointmentInterface.GetAllAppointments());
        }


    }
}
