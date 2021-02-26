using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class AdminModel
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
