using System;
using System.Collections.Generic;

namespace Nedelko.Polyclinic.Models
{
    public class Doctor
    {
        public Guid Id { get; set; }
        
        public string FName { get; set; }
        
        public string LName { get; set; }
        
        public int Age { get; set; }
        
        public int Code { get; set; }
        
        public DateTime HiredDate { get; set; }
        
        public DateTime? RetiredDate { get; set; }

        // Department FK
        public int? DepartmentId { get; set; }

        public Department Department { get; set; }

        // Area FK
        public int? AreaId { get; set; }

        public Area Area { get; set; }

        // Visits
        public List<Visit> Visits { get; set; }

        // Prescriptions
        public List<Prescription> Prescriptions { get; set; }

        // CalendarEvents
        public List<Event> CalendarEvents { get; set; }
    }
}
