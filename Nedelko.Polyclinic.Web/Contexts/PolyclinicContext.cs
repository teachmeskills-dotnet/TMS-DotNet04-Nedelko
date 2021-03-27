using Nedelko.Polyclinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nedelko.Polyclinic.Contexts
{
    public class PolyclinicContext : IdentityDbContext<User>
    {
        public PolyclinicContext(DbContextOptions<PolyclinicContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<Area> Areas { get; set; }
        
        public DbSet<Patient> Patients { get; set; }
        
        public DbSet<Visit> Visits { get; set; }
        
        public DbSet<PatientAddress> PatientAddresses { get; set; }
        
        public DbSet<Prescription> Prescriptions { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }
        
        public DbSet<Event> Events { get; set; }
    }
}
