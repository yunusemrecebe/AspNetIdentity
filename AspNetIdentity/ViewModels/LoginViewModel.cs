using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "Email adresi boş bırakılamaz!")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Parola en az 3 karakterden oluşmalıdır!")]
        public string Password { get; set; }
    }
}
