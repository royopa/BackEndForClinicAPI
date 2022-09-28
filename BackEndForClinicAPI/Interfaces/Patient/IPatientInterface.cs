using BackEndForClinicAPI.DTOs;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IPatientInterface
    {
        public Task<ActionResult<List<Patient>>> GetPatients();
        public Task<ActionResult<Patient>?> GetPatient(Guid id);
        public Task<ActionResult<Patient>> AddPatient(PatientRequestModel addPatientRequest);
        public Task<ActionResult<Patient>?> DeletePatient(Guid id);
        public Task<ActionResult<Patient>?> UpdatePatient(Guid id, PatientRequestModel updatePatientRequest);
        public IQueryable<AppointmentsDTO> GetPreviousAppointments([FromRoute] Guid id);

    }
}
