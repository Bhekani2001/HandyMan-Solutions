using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace HandyMan_Solutions.Controllers
{
    public class EmployeeOnBoardingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult OnboardNewEmployee()
        {
            var roles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            var viewModel = new EmployeeOnBoardingViewModel
            {
                Roles = roles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OnboardNewEmployee(EmployeeOnBoardingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var password = string.IsNullOrEmpty(model.Employee.Password)
                    ? userManager.PasswordHasher.HashPassword(Guid.NewGuid().ToString("n").Substring(0, 8))
                    : model.Employee.Password;

                var user = new ApplicationUser
                {
                    UserName = model.Employee.EEmailAddress,
                    Email = model.Employee.EEmailAddress,
                    FirstName = model.Employee.EFirstName,
                    LastName = model.Employee.ELastName,
                    FamilyName = model.Employee.EFamilyName,
                    Contact = model.Employee.EContact,
                    PhoneNumber = model.Employee.ESecondContact,
                    Experience = model.Employee.EYearsofExperience,
                    IDNo = model.Employee.EIdentityNumber
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user.Id, db.Roles.Find(model.Employee.RoleId).Name);

                    SendPasswordEmail(model.Employee.EEmailAddress, password);

                    return RedirectToAction("Index","Home");
                }

                AddErrors(result);
            }

            model.Roles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            return View(model);
        }

        private void SendPasswordEmail(string email, string password)
        {
            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Welcome to HandyMan Solutions!";
            message.Body = $"Your account has been created. Your temporary password is: {password}";
            message.From = new MailAddress("towandile461@gmail.com"); 
            message.IsBodyHtml = false;

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("towandile461@gmail.com", "cxtyldjbvwwgckig");
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}