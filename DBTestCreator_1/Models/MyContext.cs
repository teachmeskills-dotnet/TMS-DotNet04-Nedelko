using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class MyContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<PatientAddress> PatientAddresses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
    }
}
