using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class PrescriptionModel
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
        public string Comments { get; set; }
        [Required]
        [Display(Name = "Date of Expiration")]
        [DataType(DataType.Date)]
        public DateTime ValidTill { get; set; }
        // Patient FK
        public Guid PatientId { get; set; }
        // Doctor FK
        public Guid DoctorId { get; set; }
        // Visit Id
        public Guid? VisitId { get; set; }
    }
}
