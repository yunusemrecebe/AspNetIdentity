using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email boş bırakılamaz!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresi formatlarına uyunuz!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola boş bırakılamaz!")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
