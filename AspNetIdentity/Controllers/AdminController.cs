using AspNetIdentity.Models;
using AspNetIdentity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AspNetIdentity.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager, null, roleManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole appRole = new AppRole();
            appRole.Name = roleViewModel.Name;

            IdentityResult identityResult = _roleManager.CreateAsync(appRole).Result;

            if (identityResult.Succeeded)
            {
                return RedirectToAction("Roles");
            }

            AddModelError(identityResult);

            return View(roleViewModel);
        }

        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            AppRole appRole = _roleManager.FindByIdAsync(id).Result;

            if (appRole != null)
            {
                IdentityResult identityResult = _roleManager.DeleteAsync(appRole).Result;
            }
            return RedirectToAction("Roles");
        }

        public IActionResult RoleUpdate(string id)
        {
            AppRole appRole = _roleManager.FindByIdAsync(id).Result;

            if (appRole != null)
            {
                return View(appRole.Adapt<RoleViewModel>());
            }
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                AddModelError(result);
            }
            ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu!");
            return View(roleViewModel);
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
    }
}
