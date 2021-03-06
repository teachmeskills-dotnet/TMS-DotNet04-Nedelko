using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateReservation { get; set; }
        // Patient FK
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        // Doctor FK
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
