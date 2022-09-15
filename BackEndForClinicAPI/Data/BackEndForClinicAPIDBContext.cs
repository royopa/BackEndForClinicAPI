using BackEndForClinicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Data
{
    public class BackEndForClinicAPIDBContext : DbContext
    {
        public BackEndForClinicAPIDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
