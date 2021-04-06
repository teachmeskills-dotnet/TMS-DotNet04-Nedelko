using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class AdminViewModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Missed Email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Missed Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Pass do not confirmed.")]
        public string ConfirmPassword { get; set; }
    }
}
