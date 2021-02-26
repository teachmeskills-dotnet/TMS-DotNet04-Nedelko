using DBTestCreator_1.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class PatientModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [MaxLength(20)]
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [MaxLength(20)]
        [Display(Name ="Last Name")]
        public string LName { get; set; }

        [Display(Name ="Age")]
        public int Age { get; set; }

        [Display(Name ="Date of Birth")]
        public DateTime BDate { get; set; }

        [Display(Name ="Status")]
        public int Status { get; set; }

        [Display(Name ="Area")]
        public int AreaId { get; set; }

        public static PatientStatus GetStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return PatientStatus.Adult;
                    break;
                case 2:
                    return PatientStatus.Teenager;
                    break;
                case 3:
                    return PatientStatus.Child;
                    break;
                case 4:
                    return PatientStatus.Pensioner;
                    break;
                default:
                    return PatientStatus.Unknown;
            }
        }
    }
}
