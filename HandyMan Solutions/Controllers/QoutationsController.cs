using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class QoutationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly string site = "https://sandbox.payfast.co.za/eng/process?";
        private readonly string merchant_id = "10000100";
        private readonly string merchant_key = "46f0cd694581a";

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
            ViewBag.ServiceOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Carpentry", Text = "Carpentry" },
                new SelectListItem { Value = "Electrical Work", Text = "Electrical Work" },
                new SelectListItem { Value = "General Home Repairs", Text = "General Home Repairs" },
                new SelectListItem { Value = "Painting", Text = "Painting" },
                new SelectListItem { Value = "Plumbing", Text = "Plumbing" }
            };
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RequestQuotation(Qoutation model, HttpPostedFileBase ImageFile)
        {
            ViewBag.ServiceOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Carpentry", Text = "Carpentry" },
                new SelectListItem { Value = "Electrical Work", Text = "Electrical Work" },
                new SelectListItem { Value = "General Home Repairs", Text = "General Home Repairs" },
                new SelectListItem { Value = "Painting", Text = "Painting" },
                new SelectListItem { Value = "Plumbing", Text = "Plumbing" }
            };
            return View(model);
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