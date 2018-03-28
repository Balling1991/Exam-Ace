using ExamAce.Data.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExamAce.Data.Entities
{
    public class ApiDbSeedData
    {
        public ApiDbSeedData(UserManager<User> userManager)
        {
        }

        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAndClaims(userManager, roleManager);
            await SeedAdmin(userManager);
        }

        private static async Task SeedRolesAndClaims(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Extensions.AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Extensions.AdminRole
                });
            }

            if (!await roleManager.RoleExistsAsync(Extensions.UserRole))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = Extensions.UserRole
                });
            }

            var adminRole = await roleManager.FindByNameAsync(Extensions.AdminRole);
            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);

            if (!adminRoleClaims.Any(x => x.Type == Extensions.ManageUserClaim))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(Extensions.ManageUserClaim, "true"));
            }
            if (!adminRoleClaims.Any(x => x.Type == Extensions.AdminClaim))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(Extensions.AdminClaim, "true"));
            }

            var userRole = await roleManager.FindByNameAsync(Extensions.UserRole);
            var userRoleClaims = await roleManager.GetClaimsAsync(userRole);
            if (!userRoleClaims.Any(x => x.Type == Extensions.UserClaim))
            {
                await roleManager.AddClaimAsync(userRole, new System.Security.Claims.Claim(Extensions.UserClaim, "true"));
            }
        }

        private static async Task SeedAdmin(UserManager<User> userManager)
        {
            var u = await userManager.FindByNameAsync("admin");
            if (u == null)
            {
                u = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.dk",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsEnabled = true,
                    FirstName = "admin",
                    LastName = "user"
                };
                var x = await userManager.CreateAsync(u, "Admin1234!");
            }
            var uc = await userManager.GetClaimsAsync(u);
            if (!uc.Any(x => x.Type == Extensions.AdminClaim))
            {
                await userManager.AddClaimAsync(u, new System.Security.Claims.Claim(Extensions.AdminClaim, true.ToString()));
            }
            if (!await userManager.IsInRoleAsync(u, Extensions.AdminRole))
                await userManager.AddToRoleAsync(u, Extensions.AdminRole);
        }
    }
}
