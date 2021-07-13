using AspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetIdentity.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        public UserManager<AppUser> _userManager { get; set; }

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = principal.Identity as ClaimsIdentity;

                AppUser user = await _userManager.FindByNameAsync(claimsIdentity.Name);

                if (user != null)
                {
                    if (user.BirthDay != null)
                    {
                        var today = DateTime.Today;
                        var age = today.Year - user.BirthDay.Year;

                        if (age > 15)
                        {
                            Claim violance = new Claim("Violence", true.ToString(), ClaimValueTypes.String, "Internal");

                            claimsIdentity.AddClaim(violance);
                        }


                    }

                    if (user.City != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "City"))
                        {
                            Claim City = new Claim("City", user.City, ClaimValueTypes.String, "Internal");

                            claimsIdentity.AddClaim(City);
                        }
                    }
                }
            }
            return principal;
        }
    }
}
