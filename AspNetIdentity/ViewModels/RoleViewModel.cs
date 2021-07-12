using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol ismi boş bırakılamaz!")]
        [Display(Name = "Rol İsmi")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
