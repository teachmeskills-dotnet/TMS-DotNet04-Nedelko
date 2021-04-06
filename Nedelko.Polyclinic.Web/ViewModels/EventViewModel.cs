using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class EventViewModel
    {
        [Required]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        
        public string Text { get; set; }

        public Guid? PatientId { get; set; }

        public Guid? DoctorId { get; set; }
    }
}
