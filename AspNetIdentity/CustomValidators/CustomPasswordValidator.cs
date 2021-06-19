using AspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetIdentity.CustomValidators
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> identityErrors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                identityErrors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Parola içerisinde kullanıcı adı kullanılamaz!" });
            }

            if (password.ToLower().Contains("123"))
            {
                identityErrors.Add(new IdentityError { Code = "PasswordContainsConsecutiveNumbers", Description = "Parola içerisinde ardışık sayılar kullanılamaz!" });
            }

            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                identityErrors.Add(new IdentityError { Code = "PasswordContainsEmail", Description = "Parola içerisinde email adresiniz kullanılamaz!" });
            }

            if (identityErrors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(identityErrors.ToArray()));
        }
    }
}
