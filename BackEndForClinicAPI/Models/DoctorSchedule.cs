namespace BackEndForClinicAPI.Models
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }
        public Doctor? Doctor { get; set; }
        public Appointment? Slot1 { get; set; }= null;
        public Appointment? Slot2 { get; set; } = null;
        public Appointment? Slot3 { get; set; } = null;
        public Appointment? Slot4 { get; set; } = null;
        public Appointment? Slot5 { get; set; } = null;
        public Appointment? Slot6 { get; set; } = null;
        public Appointment? Slot7 { get; set; } = null;
        public Appointment? Slot8 { get; set; } = null;
        public DateTime Date { get; set; }

    }
}
