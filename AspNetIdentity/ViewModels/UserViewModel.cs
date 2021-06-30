using AspNetIdentity.Enums;
using System;
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

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "Profil Resmi")]
        public string Picture { get; set; }

        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
    }
}
