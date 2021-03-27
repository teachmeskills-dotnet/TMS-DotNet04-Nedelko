using Nedelko.Polyclinic.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class PatientViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [MaxLength(20)]
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [MaxLength(20)]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime BDate { get; set; }

        // Different statuses for Views

        [Display(Name = "Status")]
        public int Status { get; set; }

        public PatientStatusType? EnumStatus { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }

        public static PatientStatusType GetStatus(int status)
        {
            return status switch
            {
                1 => PatientStatusType.Adult,
                2 => PatientStatusType.Teenager,
                3 => PatientStatusType.Child,
                4 => PatientStatusType.Pensioner,
                _ => PatientStatusType.Unknown,
            };
        }
    }
}
