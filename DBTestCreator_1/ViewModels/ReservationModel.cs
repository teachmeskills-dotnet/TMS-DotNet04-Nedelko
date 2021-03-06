using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class ReservationModel
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
