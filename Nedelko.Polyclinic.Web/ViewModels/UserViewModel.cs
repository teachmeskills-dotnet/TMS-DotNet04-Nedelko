using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wrong Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Pass do not fit.")]
        public string ConfirmPassword { get; set; }
    }
}
