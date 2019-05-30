using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Helpers
{
    public class IdentityHelper
    {
        internal static void SeedIdentities(DbContext context)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(Roles.Admin))
            {
                var roleresult = roleManager.Create(new IdentityRole(Roles.Admin));
            }
            if (!roleManager.RoleExists(Roles.Lector))
            {
                var roleresult = roleManager.Create(new IdentityRole(Roles.Lector));
            }
            string adminName = "admin@coolname.cz";
            string adminPassword = "CoolPassword@";
            string xxx = "xxxxxcvkjhvxkjh GITIGNORE";
            ApplicationUser admin = userManager.FindByName(adminName);
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = adminName,
                    Email = adminName,
                    EmailConfirmed = true
                };
                IdentityResult userResult = userManager.Create(admin, adminPassword);
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(admin.Id, Roles.Admin);
                }
            }
            string userName = "user@coolname.cz";
            string userPassword = "CoolPassword@user";
            ApplicationUser user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                IdentityResult userResult = userManager.Create(user, userPassword);
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, Roles.Lector);
                }
            }
        }

    }
}