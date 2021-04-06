using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class ReservationViewModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Visit")]
        public DateTime DateReservasion { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid PatientId { get; set; }
    }
}
