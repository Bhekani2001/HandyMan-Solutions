using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class GeneralsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult RateService()
        {
            var userId = User.Identity.GetUserId(); 
            var services = db.ServiceProvideds
                              .Where(s => s.UserId == userId && s.Status == "Done" && s.Rated!=true)
                              .ToList();

            ViewBag.ServiceList = new SelectList(services, "Id", "Description"); 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateService(RateServiceViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (ModelState.IsValid)
            {
                var rateService = new RateService
                {
                    UserId = User.Identity.GetUserId(),
                    UserName = $"{user.FirstName} {user.LastName} {user.FamilyName}",
                    UserEmail = $"{user.Email}",
                    ServiceId = model.ServiceId,
                    Rating = model.Rating,
                    Comments = model.Comments,
                    Rated =true
                };

                db.RateServices.Add(rateService);

                int serviceId;
                if (int.TryParse(model.ServiceId, out serviceId))
                {
                    var service = db.ServiceProvideds.Find(serviceId);
                    if (service != null)
                    {
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Service ID.");
                }

                return RedirectToAction(nameof(RateService));
            }

            return View(model);
        }

        // In your controller
        public ActionResult Chart()
        {
            var quotations = GetQuotations();

            var data = quotations
                .GroupBy(q => q.RequestedDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    PaidCount = g.Count(q => q.Paid == "Yes"),
                    NewPaidCount = g.Count(q => q.NewPaid == "Paid")
                })
                .OrderBy(d => d.Date)
                .ToList();

            var model = new ChartViewModel
            {
                Dates = data.Select(d => d.Date.ToShortDateString()).ToArray(),
                PaidCounts = data.Select(d => d.PaidCount).ToArray(),
                NewPaidCounts = data.Select(d => d.NewPaidCount).ToArray()
            };

            return View(model);
        }

        private List<Qoutation> GetQuotations()
        {
            return new List<Qoutation>();
        }


    }
}
