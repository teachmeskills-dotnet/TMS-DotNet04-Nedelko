using DBTestCreator_1.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Models
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
    }
}
