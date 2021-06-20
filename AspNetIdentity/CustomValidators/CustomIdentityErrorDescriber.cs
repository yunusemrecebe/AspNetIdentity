using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.CustomValidators
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError 
            {
                Code = "EmailAlreadyExists",
                Description = $"'{email}' email adresi sistemde kayıtlı!"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "UsernameAlreadyExists",
                Description = $"'{userName}' kullanıcı adı sistemde kayıtlı!"
            };
        }
    }
}
