using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class PatientAddressModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name ="City")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Required]
        [Display(Name ="Street")]
        [DataType(DataType.Text)]
        public string Street { get; set; }
        [Required]
        [Display(Name ="House No.")]
        public string HouseNo { get; set; }
    }
}
