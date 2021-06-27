using AspNetIdentity.Models;
using AspNetIdentity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetIdentity.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            return View(userViewModel);
        }

        public IActionResult UserEdit()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> UserEdit(UserViewModel userViewModel)
        {
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult identityResult = await _userManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                    ViewBag.status = true;
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                if (user != null)
                {
                    bool isOldPasswordTrue = _userManager.CheckPasswordAsync(user, passwordChangeViewModel.OldPassword).Result;

                    if (isOldPasswordTrue)
                    {
                        IdentityResult result = _userManager.ChangePasswordAsync(user, passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword).Result;
                        if (result.Succeeded)
                        {
                            _userManager.UpdateSecurityStampAsync(user);
                            _signInManager.SignOutAsync();
                            _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.NewPassword, true, false);
                            ViewBag.status = true;
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski parolayı hatalı girdiniz!");
                    }
                }
            }

            return View(passwordChangeViewModel);
        }
    }
}
