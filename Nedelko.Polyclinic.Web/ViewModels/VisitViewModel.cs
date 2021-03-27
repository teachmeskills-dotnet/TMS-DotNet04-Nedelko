using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class VisitViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Visit")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        public Guid? PatientId { get; set; }

        public Guid? DoctorId { get; set; }
    }
}
