using System;
using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class PatientAddressViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street")]
        [DataType(DataType.Text)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "House No.")]
        public string Number { get; set; }
    }
}
