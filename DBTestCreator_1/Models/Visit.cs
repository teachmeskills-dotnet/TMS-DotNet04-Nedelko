using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class Visit
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateOfVisit { get; set; }
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
