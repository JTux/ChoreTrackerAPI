using System;
using System.Collections.Generic;
using System.Linq;
using ChoreTracker.Data;
using ChoreTracker.WebAPI.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChoreTracker.WebAPI.Startup))]

namespace ChoreTracker.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeedDefaultRolesAndUsers();
        }

        private void SeedDefaultRolesAndUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists(RoleNames.Admin))
            {
                var adminRole = new IdentityRole(RoleNames.Admin);
                roleManager.Create(adminRole);

                var adminUser = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Mr.",
                    LastName = "Admin"
                };

                var adminPassword = "password";

                var userCreationResult = userManager.Create(adminUser, adminPassword);

                if (userCreationResult.Succeeded)
                    userManager.AddToRole(adminUser.Id, RoleNames.Admin);
            }

            if (!roleManager.RoleExists(RoleNames.User))
            {
                var userRole = new IdentityRole(RoleNames.User);
                roleManager.Create(userRole);
            }
        }
    }
}
