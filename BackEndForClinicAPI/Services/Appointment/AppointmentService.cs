using System;
using System.Linq.Expressions;
using System.Net;

using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Services
{
    public class AppointmentService : IAppointmentInterface
    {
        private readonly BackEndForClinicAPIDBContext _dbContext;


        public AppointmentService(BackEndForClinicAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        private static readonly Expression<Func<Appointment, AppointmentsDTO>> AppointmentListWithPatientInformation =
            x => new AppointmentsDTO
            {
                AppointmentId = x.Id,
                PatientId = x.PatientId,
                AppointmentTime = x.AppointmentDateTime
            };

        public async Task<ActionResult<Appointment>?> AddAppointment(AppointmentRequestModel addAppointmentRequest)
        {

            if (addAppointmentRequest.NumberOfSlots < 1 || addAppointmentRequest.NumberOfSlots > 8)
            {
                return null;
            }

            string doctorId = addAppointmentRequest.DoctorId.ToString();

            Guid guidOfDoctorId;
            var x = Guid.TryParse(doctorId, out guidOfDoctorId);

            var numberOfAppointmentsOnGivenDay = _dbContext.Appointments
                .Where(a => a.DoctorId.Equals(guidOfDoctorId) && a.AppointmentDateTime.Date == addAppointmentRequest.AppointmentDateTime.Date)
                .Count();

            //a doctor can have a maximum of 12 appointments only
            if (numberOfAppointmentsOnGivenDay > 12)
            {
                return null;
            }            

            var appointment = new Appointment()
            {
                Id = Guid.NewGuid(),
                DoctorId = addAppointmentRequest.DoctorId,
                PatientId = addAppointmentRequest.PatientId,
                AppointmentDateTime = addAppointmentRequest.AppointmentDateTime,
                NumberOfSlots = addAppointmentRequest.NumberOfSlots
            };

            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();

            return appointment;
        }


        public IQueryable<AppointmentsDTO> GetAppointmentDetails([FromRoute] Guid id)
        {
            return _dbContext.Appointments.Include(b => b.Doctor)
              .Where(b => b.Id == id)
              .Select(AppointmentListWithPatientInformation);
        }


        public async Task<ActionResult<Appointment>?> CancelAppointment(Guid id)
        {
            var appointment = await _dbContext.Appointments.FindAsync(id);

            if (appointment != null)
            {
                _dbContext.Appointments.Remove(appointment);
                await _dbContext.SaveChangesAsync();
                return appointment;
            }

            return null;
        }


        public async Task<List<Appointment>> GetMostNumberOfAppointments(DateTime date)
        {

            List<Appointment> listOfAppointmentsSortedByDoctorID = await _dbContext.Appointments
                .OrderBy(a => a.DoctorId)
                .Where(a => a.AppointmentDateTime.Day == date.Day && a.AppointmentDateTime.Month == date.Month && a.AppointmentDateTime.Year == date.Year)
                .ToListAsync();

            
            return listOfAppointmentsSortedByDoctorID;
        }


        public async Task<List<Appointment>> GetAllAppointments()
        {
            var listOfAppointments = await _dbContext.Appointments.ToListAsync();
            return listOfAppointments;
            
        }


        public async Task<List<Doctor>> GetListOfDoctorsHavingAppointmentsMoreThanSixHours()
        {
            DateTime today = DateTime.Now;

            var listOfAppointments = await _dbContext.Appointments
                .OrderBy(a => a.DoctorId)
                .Where(a => a.AppointmentDateTime.Day == today.Day && a.AppointmentDateTime.Month == today.Month && a.AppointmentDateTime.Year == today.Year)
                .ToListAsync();

            Guid currentDoctorId = listOfAppointments[0].DoctorId;
            int totalDuration = 0;

            List<ListOfDoctorsWithMaxSlotsDTO> listOfDoctorsWithMaxSlotsDTO = new List<ListOfDoctorsWithMaxSlotsDTO>();

            //foreach (Appointment appointment in listOfAppointments)
            //{
            //    if (currentDoctorId.Equals(appointment.DoctorId))
            //    {
            //        totalDuration = (appointment.NumberOfSlots * 15);

            //        if (totalDuration > 360)
            //        {
            //            AddDoctor
            //        }
            //    } else
            //    {
            //        totalDuration = 0;
            //        currentDoctorId = appointment.DoctorId;
            //        totalDuration = appointment.NumberOfSlots * 15;
            //    }


            //}

            var listOfDoctors = await _dbContext.Doctors.ToListAsync();
            return listOfDoctors;
        }
    }
}

