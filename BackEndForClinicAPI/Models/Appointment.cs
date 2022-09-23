namespace BackEndForClinicAPI.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime AppointmentDateTime { get; set; }

    }
}
