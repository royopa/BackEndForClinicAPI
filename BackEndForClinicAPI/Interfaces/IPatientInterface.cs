using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IPatientInterface
    {
        public Task<IActionResult> GetPatients();
        public Task<IActionResult> GetPatient(Guid id);
        public Task<IActionResult> AddPatient(AddPatientRequest addPatientRequest);
        public Task<IActionResult> DeletePatient(Guid id);
        public Task<IActionResult> UpdatePatient(Guid id, UpdatePatientRequest updatePatientRequest);

    }
}
