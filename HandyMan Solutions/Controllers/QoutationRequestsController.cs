using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class QoutationRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RequestQuotation()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RequestQuotation(Qoutation qoutationRequest)
        {
            return View();
        }

        [Authorize]
        public ActionResult ApproveQuotation(string id)
        {
            var userId = User.Identity.GetUserId();
            var quotation = db.QoutationRequests.FirstOrDefault(q => q.UserId == id);

            if (quotation == null)
            {
                return HttpNotFound();
            }

            quotation.Status = "Approved";
            db.SaveChanges();

            return RedirectToAction("MyQoutations");
        }

        [Authorize]
        public ActionResult CancelQuotation(string id)
        {
            var userId = User.Identity.GetUserId();
            var quotation = db.QoutationRequests.FirstOrDefault(q => q.UserId == id);

            if (quotation == null)
            {
                return HttpNotFound();
            }
            quotation.Status = "Cancelled";
            db.SaveChanges();

            return RedirectToAction("MyQoutations");
        }

        [Authorize]
        public ActionResult DeclineQuotation(string id)
        {
            var userId = User.Identity.GetUserId();
            var quotation = db.QoutationRequests.FirstOrDefault(q => q.UserId == id);

            if (quotation == null)
            {
                return HttpNotFound();
            }
            quotation.Status = "Declined";
            db.SaveChanges();

            return RedirectToAction("MyQoutations");
        }
    }
}