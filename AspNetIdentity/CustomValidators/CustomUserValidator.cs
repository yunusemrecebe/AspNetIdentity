using AspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.CustomValidators
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            string[] numbers = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            List<IdentityError> identityErrors = new List<IdentityError>();

            foreach (var item in numbers)
            {
                if (user.UserName[0].ToString() == item)
                {
                    identityErrors.Add(new IdentityError { Code = "UsernameCanNotStartsWithaNumber", Description = "Kullanıcı adı bir rakam ile başlayamaz!" });
                }
            }

            if (identityErrors.Count() == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            return Task.FromResult(IdentityResult.Failed(identityErrors.ToArray()));
        }
    }
}
