using System;

namespace Nedelko.Polyclinic.Models
{
    public class PatientAddress
    {
        public Guid Id { get; set; }
        
        public string City { get; set; }
        
        public string Street { get; set; }
        
        public string Number { get; set; }

        // Patient FK
        public Guid PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
