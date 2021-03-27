using Nedelko.Polyclinic.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class DoctorViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Enter last Name")]
        [Display(Name = "last Name")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Enter Age")]
        [Display(Name = "Age")]
        [Range(20, 70)]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Position Code")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Enter Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Hired Date")]
        public DateTime HiredDate { get; set; }

        [Display(Name = "Department")]
        public int? Department { get; set; }

        [Display(Name = "Area")]
        public int? Area { get; set; }

        public PositionType? Position { get; set; }

        public static PositionType GetPosition(int id)
        {
            return id switch
            {
                1 => PositionType.Doctor,
                2 => PositionType.Assistent,
                3 => PositionType.Common,
                _ => PositionType.Administartive,
            };
        }
    }
}
