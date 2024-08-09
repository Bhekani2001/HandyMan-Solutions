using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HandyMan_Solutions.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public int Experience { get; set; }
        public string IDNo { get; set; }
        public decimal Balance { get; set; } = 0;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<QoutationRequest> QoutationRequests { get; set; }
        public virtual DbSet<RequestQoutation>  RequestQoutations { get; set; }
    }
}