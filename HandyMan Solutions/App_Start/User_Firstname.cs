using HandyMan_Solutions.Models;
using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HandyMan_Solutions.App_Start
{
    public class UserFirstName
    {
        public static decimal GetBalance(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                return user?.Balance ?? 0;
            }
        }

        public static string GetFirstName(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return "User";
                }

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roles = userManager.GetRoles(userId);

                if (roles.Contains("Admin"))
                {
                    return "Admin";
                }

                return user.FirstName ?? "User";
            }
        }
    }
}
