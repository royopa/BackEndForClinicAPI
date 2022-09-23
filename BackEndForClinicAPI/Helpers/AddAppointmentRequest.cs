using BackEndForClinicAPI.Models;

namespace BackEndForClinicAPI.Helpers
{
    public class AddAppointmentRequest
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
