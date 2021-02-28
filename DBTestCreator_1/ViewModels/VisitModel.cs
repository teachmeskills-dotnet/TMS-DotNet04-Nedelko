using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class VisitModel
    {
        public Guid? Id { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Visit")]
        public DateTime DateOfVisit { get; set; }
        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        // Patient
        public Guid? PatientId { get; set; }
        //Doctor
        public Guid? DoctorId { get; set; }
    }
}
