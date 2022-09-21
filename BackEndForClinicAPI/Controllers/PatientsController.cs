using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller, IPatientInterface
    {

        private readonly BackEndForClinicAPIDBContext dbContext;

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
                EmailAddress = addPatientRequest.Email,
                Phone = addPatientRequest.Phone
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
                patient.EmailAddress = updatePatientRequest.Email;
                patient.Phone = updatePatientRequest.Phone;
                patient.Address = updatePatientRequest.Address;
                patient.FullName = updatePatientRequest.FullName;
                await dbContext.SaveChangesAsync();

                return Ok(patient);
            }

            return NotFound();
        }
    }
}
