using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "Email adresi boş bırakılamaz!")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Yeni Parola")]
        [Required(ErrorMessage = "Parola boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Parola en az 3 karakterden oluşmalıdır!")]
        public string NewPassword { get; set; }
    }
}
