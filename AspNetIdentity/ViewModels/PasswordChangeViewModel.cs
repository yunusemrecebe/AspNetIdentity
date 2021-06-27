using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski Parola")]
        [Required(ErrorMessage = "Eski parola boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Parolanız en az 3 karakter içermelidir!")]
        public string OldPassword { get; set; }

        [Display(Name = "Yeni Parola")]
        [Required(ErrorMessage = "Yeni parola boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Parolanız en az 3 karakter içermelidir!")]
        public string NewPassword { get; set; }

        [Display(Name = "Yeni Parola Doğrulama")]
        [Required(ErrorMessage = "Yeni parola doğrulama boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Parolanız en az 3 karakter içermelidir!")]
        [Compare("NewPassword", ErrorMessage = "Parolalar uyuşmuyor!")]
        public string NewPasswordConfirm { get; set; }
    }
}
