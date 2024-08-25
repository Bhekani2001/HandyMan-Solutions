using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity; // Ensure this is included for User.Identity methods

namespace HandyMan_Solutions.Controllers
{
    public class AssignTasksController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult SelectQuotationAndEmployee()
        {
            var viewModel = new SelectQuotationAndEmployeeViewModel
            {
                Quotations = db.QoutationRequests
                               .Where(q => q.Status == "Approved")
                               .ToList(),
                Employees = db.EmployeeOnBoardings
                              .Select(e => new EmployeeViewModel
                              {
                                  EmployeeKey = e.EmployeeKey,
                                  EFirstName = e.EFirstName,
                                  ELastName = e.ELastName,
                                  EFamilyName = e.EFamilyName,
                                  RoleName = db.Roles
                                               .Where(r => r.Id == e.RoleId)
                                               .Select(r => r.Name)
                                               .FirstOrDefault()
                              })
                              .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AssignEmployeeToQuotation(int quotationId, int employeeKey)
        {
            var quotation = db.QoutationRequests.Find(quotationId);
            if (quotation == null)
            {
                return Json(new { success = false, message = "Quotation not found." });
            }

            var employee = db.EmployeeOnBoardings.Find(employeeKey);
            if (employee == null)
            {
                return Json(new { success = false, message = "Employee not found." });
            }

            quotation.TechnicianAssigned = employeeKey.ToString(); // Or another appropriate property
            db.SaveChanges();

            return Json(new { success = true, message = "Employee assigned successfully." });
        }

        [HttpGet]
        public ActionResult QoutationDuties()
        {
            var userId = User.Identity.GetUserId(); // Get the current logged-in user's ID
            var employee = db.EmployeeOnBoardings.FirstOrDefault(e => e.EEmailAddress == User.Identity.Name); // Retrieve the employee record based on the user's email

            if (employee == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Employee record not found.");
            }

            var employeeKey = employee.EmployeeKey;

            var quotations = db.QoutationRequests
                .Where(q => q.TechnicianAssigned == employeeKey.ToString())
                .ToList();

            // Pass the list of quotations to the view
            return View(quotations);
        }
    }
}
