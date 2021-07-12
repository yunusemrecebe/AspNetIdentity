using AspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class CustomUserRoleName : TagHelper
    {
        public UserManager<AppUser> _userManager { get; set; }

        [HtmlAttributeName("user-roles")]
        public string _userId { get; set; }

        public CustomUserRoleName(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user = _userManager.FindByIdAsync(_userId).Result;
            IList<string> roles = _userManager.GetRolesAsync(user).Result;

            string html = string.Empty;
            roles.ToList().ForEach(x =>
            {
                html += $"<span class='badge badge-info'> {x} </span> ";
            });

            output.Content.SetHtmlContent(html);
            return Task.CompletedTask;
        }
    }
}
