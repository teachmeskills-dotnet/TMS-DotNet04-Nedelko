using DBTestCreator_1.Enums;
using DBTestCreator_1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class RegDoctorModel
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

        public Positions? Position { get; set; }


        public static Positions GetPosition(int id)
        {
            if(id == 1)
            {
                return Positions.Doctor;
            }
            else if(id == 2)
            {
                return Positions.Assistent;
            }
            else if(id == 3)
            {
                return Positions.Common;
            }
            else
            {
                return Positions.Administartive;
            }
        }
    }
}
