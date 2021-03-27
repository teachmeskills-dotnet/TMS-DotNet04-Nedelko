using System;

namespace Nedelko.Polyclinic.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        public DateTime Start { get; set; }
        
        public DateTime End { get; set; }
        
        public string Text { get; set; }
        
        public string Color { get; set; }

        //Patient FK
        public Guid? PatientId { get; set; }

        public Patient Patient { get; set; }

        //Doctor FK
        public Guid? DoctorId { get; set; }

        public Doctor Doctor { get; set; }
    }
}
