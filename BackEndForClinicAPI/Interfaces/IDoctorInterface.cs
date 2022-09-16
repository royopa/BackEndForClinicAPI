using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IDoctorInterface
    {
        public Task<IActionResult> GetDoctors();
        public Task<IActionResult> GetDoctor(Guid id);
        public Task<IActionResult> AddDoctor(AddDoctorRequest addDoctorRequest);
        public Task<IActionResult> DeleteDoctor(Guid id);
        public Task<IActionResult> UpdateDoctor(Guid id,UpdateDoctorRequest updateDoctorRequest);

    }
}
