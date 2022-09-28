using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IDoctorInterface
    {
        public Task<List<Doctor>> GetDoctors();
        public Task<Doctor?> GetDoctor(Guid id);
        public Task<ActionResult<Doctor>> AddDoctor(DoctorRequestModel addDoctorRequest);
        public Task<ActionResult<Doctor>?> DeleteDoctor(Guid id);
        public Task<ActionResult<Doctor>?> UpdateDoctor(Guid id,DoctorRequestModel updateDoctorRequest);
        public IQueryable<AppointmentsDTO> GetSchedule([FromRoute] Guid id);
        public IQueryable<AppointmentsDTO> GetScheduleWithoutPatientInformation([FromRoute] Guid id);
        

    }
}
