﻿using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
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
                EmailAddress = addPatientRequest.EmailAddress,
                Phone = addPatientRequest.Phone,
                GivenName = addPatientRequest.GivenName,
                InsuranceNumber = addPatientRequest.InsuranceNumber,
                Password = addPatientRequest.Password,
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
                await dbContext.SaveChangesAsync();

                return Ok(patient);
            }

            return NotFound();
        }
    }
}
