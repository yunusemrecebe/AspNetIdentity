using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "Email adresi boş bırakılamaz!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
