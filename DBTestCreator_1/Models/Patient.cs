using DBTestCreator_1.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
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
        public PatientStatus Status { get; set; }
        // Area FK
        public int AreaId { get; set; }
        public Area Area { get; set; }
        // Visits
        public List<Visit> Visits { get; set; }
        // Prescription
        public List<Prescription> Prescriptions { get; set; }

    }
}
