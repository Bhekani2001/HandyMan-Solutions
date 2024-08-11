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

        public ActionResult ExistingEmployees()
        {
            var employees = db.EmployeeOnBoardings.ToList();
            return View(employees);
        }

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
                    ? Guid.NewGuid().ToString("n").Substring(0, 8)
                    : model.Employee.Password;

                var user = new ApplicationUser
                {
                    UserName = model.Employee.EEmailAddress,
                    Email = model.Employee.EEmailAddress,
                    FirstName = model.Employee.EFirstName,
                    LastName = model.Employee.ELastName,
                    FamilyName = model.Employee.EFamilyName,
                    Address = model.Employee.EAddress,
                    SecondContact = model.Employee.ESecondContact,
                    PhoneNumber = model.Employee.EContact,
                    Experience = model.Employee.EYearsofExperience,
                    IDNo = model.Employee.EIdentityNumber,
                    RegisteredDate = DateTime.Now,
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var role = db.Roles.Find(model.Employee.RoleId).Name;
                    await userManager.AddToRoleAsync(user.Id, role);

                    var employeeOnBoarding = new EmployeeOnBoarding
                    {
                        EFirstName = model.Employee.EFirstName,
                        ELastName = model.Employee.ELastName,
                        EFamilyName = model.Employee.EFamilyName,
                        EIdentityNumber = model.Employee.EIdentityNumber,
                        EContact = model.Employee.EContact,
                        ESecondContact = model.Employee.ESecondContact,
                        EAddress = model.Employee.EAddress,
                        EEmailAddress = model.Employee.EEmailAddress,
                        EYearsofExperience = model.Employee.EYearsofExperience,
                        RoleId = model.Employee.RoleId,
                    };

                    // Save the EmployeeOnBoarding entity
                    db.EmployeeOnBoardings.Add(employeeOnBoarding);
                    db.SaveChanges();

                    // Send the password email
                    SendPasswordEmail(model.Employee.EEmailAddress, password);

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed; redisplay the form
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