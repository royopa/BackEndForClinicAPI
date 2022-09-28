using System;
using System.Linq.Expressions;
using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Services
{
    public class DoctorService : IDoctorInterface
    {
        private readonly BackEndForClinicAPIDBContext _dbContext;


        public DoctorService(BackEndForClinicAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ActionResult<Doctor>> AddDoctor(DoctorRequestModel addDoctorRequest)
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

            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            return doctor;
        }


        public async Task<ActionResult<Doctor>?> DeleteDoctor(Guid id)
        {
            var doctor = await _dbContext.Doctors.FindAsync(id);

            if (doctor != null)
            {
                _dbContext.Doctors.Remove(doctor);
                await _dbContext.SaveChangesAsync();
                return doctor;
            }

            return null;
        }


        private static readonly Expression<Func<Appointment, AppointmentsDTO>> AppointmentListWithPatientInformation =
            x => new AppointmentsDTO
            {
                AppointmentId = x.Id,
                PatientId = x.PatientId,
                AppointmentTime = x.AppointmentDateTime
            };


        private static readonly Expression<Func<Appointment, AppointmentsDTO>> AppointmentListWithoutPatientInformation =
            x => new AppointmentsDTO
            {
                AppointmentId = x.Id,
                AppointmentTime = x.AppointmentDateTime
            };


        //Pass in the Doctor ID - guid
        public IQueryable<AppointmentsDTO> GetSchedule([FromRoute] Guid id)
        {
            return _dbContext.Appointments.Include(b => b.Doctor)
                .Where(b => b.DoctorId == id)
                .Select(AppointmentListWithPatientInformation);
        }


        //Pass in the Doctor ID - guid
        public IQueryable<AppointmentsDTO> GetScheduleWithoutPatientInformation([FromRoute] Guid id)
        {
            return _dbContext.Appointments.Include(b => b.Doctor)
                .Where(b => b.DoctorId == id)
                .Select(AppointmentListWithoutPatientInformation);
        }


        public async Task<Doctor?> GetDoctor(Guid id)
        {
            var doctor = await _dbContext.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }


        public async Task<List<Doctor>> GetDoctors()
        {
            var listOfDoctors = await _dbContext.Doctors.ToListAsync();
            return listOfDoctors;
        }


        public async Task<ActionResult<Doctor>?> UpdateDoctor(Guid id, DoctorRequestModel updateDoctorRequest)
        {
            var doctor = await _dbContext.Doctors.FindAsync(id);

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
                await _dbContext.SaveChangesAsync();

                return doctor;
            }

            return null;
        }

        
    }
}

