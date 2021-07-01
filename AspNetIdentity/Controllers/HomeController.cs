using AspNetIdentity.Models;
using AspNetIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetIdentity.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager):base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }

            return View();
        }

        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "3 defa hatalı giriş denemesi yaptığınızdan ötürü hesabınız bir süre kilitli kalacaktır!");

                        return View(loginViewModel);
                    }

                    await _signInManager.SignOutAsync();
                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);

                    if (signInResult.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member");
                    }
                    await _userManager.AccessFailedAsync(user);
                    int accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);

                    if (accessFailedCount == 3)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));

                        ModelState.AddModelError("", "3 Defa hatalı giriş denemesi yaptığınızdan ötürü hesabınız 20 dakika boyunca kilitli kalacaktır!");
                    }
                    else
                    {
                        ModelState.AddModelError("", $"{3 - accessFailedCount} defa hatalı deneme hakkınız kalmıştır.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı!");
                }
            }

            return View(loginViewModel);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();
                appUser.UserName = userViewModel.UserName;
                appUser.Email = userViewModel.Email;
                appUser.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                AddModelError(result);
            }

            return View(userViewModel);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = _userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
            if (user != null)
            {
                string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new { userId = user.Id, token = passwordResetToken }, HttpContext.Request.Scheme);
                Helpers.PasswordReset.PasswordResetSendEmail(passwordResetLink);
                ViewBag.status = "Success";
            }
            else
            {
                ModelState.AddModelError("", "Sistemde böyle bir email adresi bulunamadı!");
            }

            return View(passwordResetViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["resetPasswordConfirmToken"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("NewPassword")] PasswordResetViewModel passwordResetViewModel)
        {
            string userId = TempData["userId"].ToString();
            string token = TempData["resetPasswordConfirmToken"].ToString();

            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, passwordResetViewModel.NewPassword);

                if (identityResult.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                    TempData["passwordResetInfo"] = "Parola başarıyla sıfırlanmıştır. Yeni parolanız ile giriş yapabilirsiniz";
                }
                else
                {
                    AddModelError(identityResult);
                }
            }
            else
            {
                ModelState.AddModelError("", "Bir hata oluştu!");
            }

            return View(passwordResetViewModel);
        }
    }
}
