using System;
using System.Collections.Generic;

namespace Nedelko.Polyclinic.Models
{
    public class Visit
    {
        public Guid Id { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Diagnosis { get; set; }

        // Patient FK
        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

        // Doctor FK
        public Guid DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        // Prescriptions
        public List<Prescription> Prescrptions { get; set; }
    }
}
