using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller, IPatientInterface
    {

        private readonly BackEndForClinicAPIDBContext dbContext;

        private static readonly Expression<Func<Doctor, DoctorListDTO>> AsDoctorsInTheClinicList =
            x => new DoctorListDTO
            {
                DoctorName = x.Surname,
                PhoneNumber = x.Phone,
                Specialty = x.Specialty
            };


        public PatientsController(BackEndForClinicAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpPost]
        public async Task<IActionResult> AddPatient(AddPatientRequest addPatientRequest)
        {
            var patient = new Patient()
            {
                Id = Guid.NewGuid(),
                Address = addPatientRequest.Address,
                FullName = addPatientRequest.FullName,
                EmailAddress = addPatientRequest.EmailAddress,
                Phone = addPatientRequest.Phone,
                GivenName = addPatientRequest.GivenName,
                InsuranceNumber = addPatientRequest.InsuranceNumber,
                Password = PasswordEncrypterDecrypter.EncryptPassword(addPatientRequest.Password.ToString().ToLower()),
                Role = Roles.PATIENT.ToString(),
                Surname = addPatientRequest.Surname,
                UserName = addPatientRequest.UserName
            };

            await dbContext.Patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();

            return Ok(patient);
        }



        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePatient([FromRoute] Guid id)
        {
            var patient = await dbContext.Patients.FindAsync(id);

            if (patient != null)
            {
                dbContext.Patients.Remove(patient);
                await dbContext.SaveChangesAsync();
                return Ok(patient);
            }

            return NotFound();
        }



        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetPatient([FromRoute] Guid id)
        {
            var patient = await dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.Password = PasswordEncrypterDecrypter.DecryptPassword(patient.Password);
            return Ok(patient);
        }



        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            return Ok(await dbContext.Patients.ToListAsync());
        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, UpdatePatientRequest updatePatientRequest)
        {
            var patient = await dbContext.Patients.FindAsync(id);

            if (patient != null)
            {
                patient.EmailAddress = updatePatientRequest.EmailAddress;
                patient.Phone = updatePatientRequest.Phone;
                patient.Address = updatePatientRequest.Address;
                patient.FullName = updatePatientRequest.FullName;
                patient.Surname = updatePatientRequest.Surname;
                patient.UserName = updatePatientRequest.UserName;
                patient.InsuranceNumber = updatePatientRequest.InsuranceNumber;
                patient.GivenName = updatePatientRequest.GivenName;
                patient.Password = PasswordEncrypterDecrypter.EncryptPassword(updatePatientRequest.Password.ToString().ToLower());
                await dbContext.SaveChangesAsync();

                return Ok(patient);
            }

            return NotFound();
        }


        [HttpGet("doctors-list")]
        public IQueryable<DoctorListDTO> GetListOfDoctors()
        {
            return dbContext.Doctors
                .Select(AsDoctorsInTheClinicList);
        }
    }
}
