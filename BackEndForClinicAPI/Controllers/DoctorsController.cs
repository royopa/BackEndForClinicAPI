using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : Controller, IDoctorInterface
    {
        private readonly BackEndForClinicAPIDBContext dbContext;

        public DoctorsController(BackEndForClinicAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            return Ok(await dbContext.Doctors.ToListAsync());
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetDoctor([FromRoute] Guid id)
        {
            var doctor = await dbContext.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            doctor.Password = PasswordEncrypterDecrypter.DecryptPassword(doctor.Password);
            return Ok(doctor);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddDoctor(AddDoctorRequest addDoctorRequest)
        {
            var doctor = new Doctor()
            {
                Id = Guid.NewGuid(),
                GivenName = addDoctorRequest.GivenName,
                Address = addDoctorRequest.Address,
                FullName = addDoctorRequest.FullName,
                EmailAddress = addDoctorRequest.EmailAddress,
                Phone = addDoctorRequest.Phone,
                Surname = addDoctorRequest.Surname,
                Role = Roles.DOCTOR.ToString(),
                Specialty = addDoctorRequest.Specialty,
                UserName = addDoctorRequest.UserName,
                Password = PasswordEncrypterDecrypter.EncryptPassword(addDoctorRequest.Password.ToString().ToLower())
            };

            await dbContext.Doctors.AddAsync(doctor);
            await dbContext.SaveChangesAsync();

            return Ok(doctor);
            
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
        {
            var doctor = await dbContext.Doctors.FindAsync(id);

            if (doctor != null)
            {
                dbContext.Doctors.Remove(doctor);
                await dbContext.SaveChangesAsync();
                return Ok(doctor);
            }

            return NotFound();
        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, UpdateDoctorRequest updateDoctorRequest)
        {
            var doctor = await dbContext.Doctors.FindAsync(id);

            if (doctor != null)
            {
                doctor.Specialty = updateDoctorRequest.Specialty;
                doctor.Surname = updateDoctorRequest.Surname;
                doctor.GivenName = updateDoctorRequest.GivenName;
                doctor.UserName = updateDoctorRequest.UserName;
                doctor.Password = PasswordEncrypterDecrypter.EncryptPassword(updateDoctorRequest.Password.ToString().ToLower());
                doctor.Role = Roles.DOCTOR.ToString();
                doctor.EmailAddress = updateDoctorRequest.EmailAddress;
                doctor.Phone = updateDoctorRequest.Phone;
                doctor.Address = updateDoctorRequest.Address;
                doctor.FullName = updateDoctorRequest.FullName;
                await dbContext.SaveChangesAsync();

                return Ok(doctor);
            }

            return NotFound();
        }

        
        private static readonly Expression<Func<Appointment, AppointmentsDTO>> AsAppointmentListDto =
            x => new AppointmentsDTO
            {
                AppointmentId = x.Id,
                PatientId = x.PatientId,
                AppointmentTime = x.AppointmentDateTime
            };


        [HttpGet("{id:guid}/slots")]
        public IQueryable<AppointmentsDTO> GetAppointments([FromRoute] Guid id)
        {

            return dbContext.Appointments.Include(b => b.Doctor)
                .Where(b => b.DoctorId == id)
                .Select(AsAppointmentListDto);
        }
    }
}
