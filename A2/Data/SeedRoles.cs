using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Data
{
    public static class SeedRoles
    {
        public static void SeedIdentityRoles(RoleManager<IdentityRole> roleManager)
        {
            List<string> roleList = new List<string>()
            {
                "Admin",
                "Customer"
            };

            foreach (var role in roleList)
            {
                var result = roleManager.RoleExistsAsync(role).Result;
                if (!result)
                {
                    roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
