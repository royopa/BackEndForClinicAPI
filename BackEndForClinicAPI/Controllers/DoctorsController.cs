using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(AddDoctorRequest addDoctorRequest)
        {
            var doctor = new Doctor()
            {
                Id = Guid.NewGuid(),
                Address = addDoctorRequest.Address,
                FullName = addDoctorRequest.FullName,
                Email = addDoctorRequest.Email,
                Phone = addDoctorRequest.Phone
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
                doctor.Email = updateDoctorRequest.Email;
                doctor.Phone = updateDoctorRequest.Phone;
                doctor.Address = updateDoctorRequest.Address;
                doctor.FullName = updateDoctorRequest.FullName;
                await dbContext.SaveChangesAsync();

                return Ok(doctor);
            }

            return NotFound();
        }
    }
}
