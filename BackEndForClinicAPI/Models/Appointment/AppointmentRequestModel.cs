using BackEndForClinicAPI.Models;

namespace BackEndForClinicAPI.Helpers
{
    public class AppointmentRequestModel
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int NumberOfSlots { get; set; }
    }
}
