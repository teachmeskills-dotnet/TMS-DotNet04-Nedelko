using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class CalendarEventModel
    {
        [Required]
        [Display(Name = "Date and Time")]
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        //Patient FK
        public Guid? PatientId { get; set; }

        //Doctor FK
        public Guid? DoctorId { get; set; }
    }
}
