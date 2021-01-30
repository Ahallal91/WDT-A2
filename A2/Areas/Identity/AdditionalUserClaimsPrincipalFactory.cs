using A2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class AdditionalUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<A2User, IdentityRole>
{
    public AdditionalUserClaimsPrincipalFactory(
        UserManager<A2User> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    { }


}