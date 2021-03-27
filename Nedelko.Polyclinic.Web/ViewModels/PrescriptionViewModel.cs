using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class PrescriptionViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Cure")]
        public string Cure { get; set; }

        [Required]
        [Display(Name = "Date of Prescription")]
        [DataType(DataType.Date)]
        public DateTime DateOfPrescription { get; set; }

        [Display(Name = "Comments")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Date of Expiration")]
        [DataType(DataType.Date)]
        public DateTime ValidTill { get; set; }

        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public Guid? VisitId { get; set; }
    }
}
