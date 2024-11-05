using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class ServiceProvidesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult MyPaidOffServices()
        {
            string userId = User.Identity.GetUserId();

            var myQuotations = db.ServiceProvideds
                                   .Where(s => s.UserId == userId && s.Status== "In-Progress")
                                   .ToList();

            return View(myQuotations);
        }
        
        [HttpGet]
        public ActionResult MyServicesHistory()
        {
            string userId = User.Identity.GetUserId();

            var myQuotations = db.ServiceProvideds
                                   .Where(s => s.UserId == userId && s.Status=="Done")
                                   .ToList();

            return View(myQuotations);
        }

        public ActionResult PaidOffServices()
        {
            var service = db.ServiceProvideds
                                  .Where(sq => sq.Paid =="Yes")
                                  .ToList();
            return View(service);
        }
        
        public ActionResult AdminPaidOffServices()
        {
            var service = db.ServiceProvideds
                                  .Where(sq => sq.Paid =="Yes" && sq.Status== "In-Progress")
                                  .ToList();
            return View(service);
        }
        
        public ActionResult ServiceHistory()
        {
            var service = db.ServiceProvideds
                                  .Where(sq => sq.Status == "Done")
                                  .ToList();
            return View(service);
        }
        public ActionResult OverDueTask()
        {
            var service = db.ServiceProvideds
                                  .Where(sq => sq.EndDate<DateTime.Now & sq.Status!="Done")
                                  .ToList();
            return View(service);
        }
 
        public ActionResult ProcessTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceProvided serviceProvided = db.ServiceProvideds.Find(id);
            if (serviceProvided == null)
            {
                return HttpNotFound();
            }

            return View(serviceProvided);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ProcessTask(ServiceProvided model)
        {
            if (ModelState.IsValid)
            {
                var serviceProvided = await db.ServiceProvideds.FirstOrDefaultAsync(q => q.Id == model.Id);

                if (serviceProvided != null)
                {
                    serviceProvided.StartDate = DateTime.Now;
                    serviceProvided.EndDate = model.EndDate;

                    serviceProvided.Status = "In-Progress"; 
                    await db.SaveChangesAsync();

                    string redirectUrl = Url.Action(nameof(PaidOffServices));

                    return Json(new { success = true, message = "Task updated successfully.", redirectUrl = redirectUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Task not found." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid data." });
            }
        }

        [HttpGet]
        public ActionResult CloseTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceProvided serviceProvided = db.ServiceProvideds.Find(id);
            if (serviceProvided == null)
            {
                return HttpNotFound();
            }

            return View(serviceProvided);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CloseTask(ServiceProvided model)
        {
            if (ModelState.IsValid)
            {
                var serviceProvided = await db.ServiceProvideds.FirstOrDefaultAsync(q => q.Id == model.Id);

                if (serviceProvided != null)
                {
                    serviceProvided.EndDate = DateTime.Now;

                    serviceProvided.TechnicalNotes= serviceProvided.TechnicalNotes; 
                    serviceProvided.Status = "Done"; 
                    await db.SaveChangesAsync();

                    string redirectUrl = Url.Action(nameof(PaidOffServices));

                    return Json(new { success = true, message = "Task Completed successfully.", redirectUrl = redirectUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Task not found." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid data." });
            }
        }
        
        [HttpGet]
        public ActionResult UpdateTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceProvided serviceProvided = db.ServiceProvideds.Find(id);
            if (serviceProvided == null)
            {
                return HttpNotFound();
            }

            return View(serviceProvided);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateTask(ServiceProvided model)
        {
            if (ModelState.IsValid)
            {
                var serviceProvided = await db.ServiceProvideds.FirstOrDefaultAsync(q => q.Id == model.Id);

                if (serviceProvided != null)
                {
                    serviceProvided.EndDate = DateTime.Now;
                    serviceProvided.TechnicalNotes = serviceProvided.TechnicalNotes;
                    serviceProvided.NewTechnicalStatus = "Delayed";

                    await db.SaveChangesAsync();

                    string redirectUrl = Url.Action(nameof(PaidOffServices));

                    return Json(new { success = true, message = "Task updated successfully.", redirectUrl = redirectUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Task not found." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid data." });
            }
        }



        [HttpPost]
        public ActionResult TransferServiceToOverDue(int? id)
        {
            var service = db.ServiceProvideds.Find(id);

            if (service != null)
            {
                var overdueTask = new OverDueTask
                {
                    UserId = service.UserId,
                    UserName = service.UserName,
                    UserAddress = service.UserAddress,
                    UserContact = service.UserContact,
                    UserEmail = service.UserEmail,
                    ServiceType = service.ServiceType,
                    Description = service.Description,
                    UrgencyLevel = service.UrgencyLevel,
                    PropertyType = service.PropertyType,
                    Paid = service.Paid,
                    PaymentDate = service.PaymentDate,
                    StartDate = service.StartDate,
                    EndDate = service.EndDate,
                    EstimatedCost = service.EstimatedCost,
                    Status = "Forwarded",
                    TechnicalNotes = service.TechnicalNotes,
                    TechnicalStatus = service.TechnicalStatus,
                    TechnicianAssigned = service.TechnicianAssigned
                };

                db.OverDueTasks.Add(overdueTask);

                db.ServiceProvideds.Remove(service);

                db.SaveChanges();

                TempData["Message"] = "Task forwarded successfully!";
            }
            return RedirectToAction(nameof(OverDueTask));
        }

    }
}