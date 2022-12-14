namespace BackEndForClinicAPI.Helpers
{
    public class PatientRequestModel
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public string GivenName { get; set; }
        public string UserName { get; set; }
        public string InsuranceNumber { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
