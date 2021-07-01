using AspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentity.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> _userManager { get; }
        protected SignInManager<AppUser> _signInManager { get; }
        protected RoleManager<AppRole> _roleManager { get; set; }
        protected AppUser CurrentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public void AddModelError(IdentityResult identityResult)
        {
            foreach (var item in identityResult.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

    }
}
