using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly BackEndForClinicAPIDBContext dbContext;

        public AppointmentsController(BackEndForClinicAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AddAppointmentRequest addAppointmentRequest)
        {
            var appointment = new Appointment()
            {
                Id = Guid.NewGuid(),
                DoctorId = addAppointmentRequest.DoctorId,
                PatientId = addAppointmentRequest.PatientId,
                AppointmentDateTime = DateTime.Now
            };

            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            return Ok(appointment);

        }
    }
}
