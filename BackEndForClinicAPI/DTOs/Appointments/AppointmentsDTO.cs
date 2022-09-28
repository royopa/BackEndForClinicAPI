namespace BackEndForClinicAPI.DTOs
{
    public class AppointmentsDTO
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public Guid? PatientId { get; set; }
        public int NumberOfSlots { get; set; }
    }
}
