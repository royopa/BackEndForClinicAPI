using BackEndForClinicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Data
{
    public class BackEndForClinicAPIDBContext : DbContext
    {
        public BackEndForClinicAPIDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Admin> Admins { get; set; }


    }
}
