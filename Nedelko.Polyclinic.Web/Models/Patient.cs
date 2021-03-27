using Nedelko.Polyclinic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string FName { get; set; }

        [MaxLength(20)]
        public string LName { get; set; }

        public int Age { get; set; }
        
        public DateTime BDate { get; set; }
        
        public PatientStatusType Status { get; set; }

        // Area FK
        public int AreaId { get; set; }

        public Area Area { get; set; }

        // Visits
        public List<Visit> Visits { get; set; }

        // Prescription
        public List<Prescription> Prescriptions { get; set; }

        // CalendarEvents
        public List<Event> CalendarEvents { get; set; }
    }
}
