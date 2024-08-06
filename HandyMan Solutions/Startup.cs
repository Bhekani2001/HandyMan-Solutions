using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HandyMan_Solutions.Startup))]

namespace HandyMan_Solutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "admin@handyman.com";
                user.Email = "admin@handyman.com";
                string userPWD = "Admin@HM";

                var result = userManager.Create(user, userPWD);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            CreateRoleIfNotExists(roleManager, "Appliance Repair Technician");
            CreateRoleIfNotExists(roleManager, "HVAC Technician");
            CreateRoleIfNotExists(roleManager, "Carpenter");
            CreateRoleIfNotExists(roleManager, "Electrician");
            CreateRoleIfNotExists(roleManager, "Plumber");
            CreateRoleIfNotExists(roleManager, "General Home Maintenance");
        }

        private void CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole();
                role.Name = roleName;
                roleManager.Create(role);
            }
        }
    }
}
