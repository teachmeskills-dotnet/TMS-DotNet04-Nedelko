using System.ComponentModel.DataAnnotations;

namespace Nedelko.Polyclinic.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
