using System;

namespace Nedelko.Polyclinic.Models
{
    public class Prescription
    {
        public Guid Id { get; set; }
        
        public string Cure { get; set; }
        
        public DateTime DateOfPrescription { get; set; }
        
        public string Comment { get; set; }
        
        public DateTime ValidTill { get; set; }

        // Patient FK
        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }

        // Doctor FK
        public Guid DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        // Visit FK
        public Guid? VisitId { get; set; }

        public Visit Visit { get; set; }
    }
}
