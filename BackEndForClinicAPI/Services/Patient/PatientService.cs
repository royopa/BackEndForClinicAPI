using System;
using System.Linq.Expressions;
using System.Security.Claims;
using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Services
{
    public class PatientService : IPatientInterface
    {
        private readonly BackEndForClinicAPIDBContext _dbContext;


        public PatientService(BackEndForClinicAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        private static readonly Expression<Func<Doctor, DoctorListDTO>> AsDoctorsInTheClinicList =
            x => new DoctorListDTO
            {
                DoctorName = x.Surname,
                PhoneNumber = x.Phone,
                Specialty = x.Specialty
            };


        [HttpGet("doctors-list")]
        public IQueryable<DoctorListDTO> GetListOfDoctors()
        {
            return _dbContext.Doctors
                .Select(AsDoctorsInTheClinicList);
        }


        public async Task<ActionResult<Patient>> AddPatient(PatientRequestModel addPatientRequest)
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

            await _dbContext.Patients.AddAsync(patient);
            await _dbContext.SaveChangesAsync();

            return patient;
        }


        public async Task<ActionResult<Patient>?> DeletePatient(Guid id)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient != null)
            {
                _dbContext.Patients.Remove(patient);
                await _dbContext.SaveChangesAsync();
                return patient;
            }

            return null;
        }


        public async Task<ActionResult<Patient>?> GetPatient(Guid id)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient != null)
            {
                return patient;
            }

            return null;
        }


        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return await _dbContext.Patients.ToListAsync();
        }


        public async Task<ActionResult<Patient>?> UpdatePatient(Guid id, PatientRequestModel updatePatientRequest)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

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
                await _dbContext.SaveChangesAsync();

                return patient;
            }

            return null;
        }


        private static readonly Expression<Func<Appointment, AppointmentsDTO>> AsAppointmentListDto =
            x => new AppointmentsDTO
            {
                AppointmentId = x.Id,
                PatientId = x.PatientId,
                AppointmentTime = x.AppointmentDateTime
            };


        public IQueryable<AppointmentsDTO> GetPreviousAppointments([FromRoute] Guid id)
        {
            return _dbContext.Appointments.Include(b => b.Patient)
                .Where(b => b.PatientId == id && b.AppointmentDateTime < DateTime.Today)
                .Select(AsAppointmentListDto);

        }

        
    }
}

