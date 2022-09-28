using System;
using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IAppointmentInterface
    {
        public Task<ActionResult<Appointment>?> AddAppointment(AppointmentRequestModel addAppointmentRequest);
        public IQueryable<AppointmentsDTO> GetAppointmentDetails([FromRoute] Guid id);
        public Task<ActionResult<Appointment>?> CancelAppointment(Guid id);
        public Task<List<Appointment>> GetMostNumberOfAppointments(DateTime date);
        public Task<List<Appointment>> GetAllAppointments();
        public Task<List<Doctor>> GetListOfDoctorsHavingAppointmentsMoreThanSixHours();
    }
}

