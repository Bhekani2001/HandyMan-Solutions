using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class QoutationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [HttpGet]
        public ActionResult RequestQuotation()
        {
            ViewBag.ServiceOptions = new SelectList(new[]
            {
                "Carpentry",
                "Electrical Work",
                "General Home Repairs",
                "Painting",
                "Plumbing"
            });
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestQuotation(Qoutation model, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return Json(new { success = false, errors = new[] { "User not found." } });
            }

            model.UserId = userId;
            model.UserName = $"{user.FirstName} {user.LastName} {user.FamilyName}";
            model.RequestedDate = DateTime.Now;
            model.TechnicianAssigned = "None";
            model.Paid = "None";
            model.Status = "Submitted";
            model.EstimatedCost = 0;

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/Quotations/"), fileName);
                ImageFile.SaveAs(path);
                model.ImageUrl = "/Uploads/Quotations/" + fileName;
            }

            db.QoutationRequests.Add(model);
            db.SaveChanges();

            return Json(new { success = true });
        }

        [Authorize]
        public ActionResult MyQoutations()
        {
            var userId = User.Identity.GetUserId();
            var myQuotations = db.QoutationRequests
                                  .Where(q => q.UserId == userId)
                                  .ToList();
            return View(myQuotations);
        }

        public ActionResult AllQuotations()
        {
            var allQuotations = db.QoutationRequests.ToList();
            return View(allQuotations);
        }
    }
}
