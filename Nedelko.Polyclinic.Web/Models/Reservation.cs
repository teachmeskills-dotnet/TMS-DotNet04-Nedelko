using System;

namespace Nedelko.Polyclinic.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Date { get; set; }

        // Patient FK
        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

        // Doctor FK
        public Guid DoctorId { get; set; }

        public Doctor Doctor { get; set; }
    }
}
