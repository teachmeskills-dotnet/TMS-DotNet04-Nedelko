using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
