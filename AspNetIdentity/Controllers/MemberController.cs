using AspNetIdentity.Enums;
using AspNetIdentity.Models;
using AspNetIdentity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetIdentity.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            AppUser user = CurrentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            return View(userViewModel);
        }

        public IActionResult UserEdit()
        {
            AppUser user = CurrentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> UserEdit(UserViewModel userViewModel, IFormFile userPicture)
        {
            ModelState.Remove("Password");
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;

                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/userImages", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        user.Picture = "/images/userImages/" + fileName;
                    }
                }

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                user.BirthDay = (DateTime)userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;

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
                    AddModelError(identityResult);
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
                AppUser user = CurrentUser;
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
                            AddModelError(result);
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

        public void LogOut()
        {
            _signInManager.SignOutAsync();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            if (returnUrl.Contains("ViolencePage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet içerikli videolar barındırdığından ötürü 15 yaşından küçüklerin erişimine uygun değildir!";
            }
            else if (returnUrl.Contains("IstanbulPage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfaya yalnızca İstanbul'da ikamet eden kullanıcılar erişebilir.";
            }
            else if (returnUrl.Contains("Exchange"))
            {
                ViewBag.message = "30 günlük ücretsiz deneme hakkınız sona ermiştir!";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur. Erişim izni almak için sistem yöneticisi ile iletişime geçiniz";
            }

            return View();
        }

        [Authorize(Roles = "Editor, Admin")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Roles = "Manager, Admin")]
        public IActionResult Manager()
        {
            return View();
        }

        [Authorize(Policy = "IstanbulPolicy")]
        public IActionResult IstanbulPage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }

        public async Task<IActionResult> ExchangeRedirect()
        {
            bool result = User.HasClaim(x => x.Type == "ExpireDateExchange");

            if (!result)
            {
                Claim expireDateExchange = new Claim("ExpireDateExchange", DateTime.Now.AddDays(30).Date.ToShortDateString(), ClaimValueTypes.String, "Internal");

                await _userManager.AddClaimAsync(CurrentUser, expireDateExchange);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(CurrentUser, true);
            }

            return RedirectToAction("Exchange");
        }

        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult Exchange()
        {
            return View();
        }
    }
}
