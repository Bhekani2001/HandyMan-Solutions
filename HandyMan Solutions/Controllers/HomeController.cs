using QRCoder;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using HandyMan_Solutions.Models;
using System.Linq;
using System.Data.Entity.SqlServer;


namespace Tassc.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var registrations = db.Users
                .GroupBy(u => new { Year = SqlFunctions.DatePart("year", u.RegisteredDate), Week = SqlFunctions.DatePart("wk", u.RegisteredDate) })
                .Select(g => new
                {
                    Week = g.Key.Week.ToString(),
                    Year = g.Key.Year.ToString(),
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Week)
                .ToList();

            var model = new WeeklyUserRegistrationsViewModel
            {
                Weeks = registrations.Select(r => $"{r.Year}-W{r.Week}").ToList(),
                UserCounts = registrations.Select(r => r.Count).ToList()
            };

            return View(model);
        }




        public ActionResult FAQs()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.ReasonOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Inquiry", Text = "Inquiry" },
                new SelectListItem { Value = "Feedback", Text = "Feedback" },
                new SelectListItem { Value = "Support", Text = "Support" }
            };

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
        public ActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.LoggedOn = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
            }

            ViewBag.ReasonOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Inquiry", Text = "Inquiry" },
                new SelectListItem { Value = "Feedback", Text = "Feedback" },
                new SelectListItem { Value = "Support", Text = "Support" }
            };

            ViewBag.ServiceOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Carpentry", Text = "Carpentry" },
                new SelectListItem { Value = "Electrical Work", Text = "Electrical Work" },
                new SelectListItem { Value = "General Home Repairs", Text = "General Home Repairs" },
                new SelectListItem { Value = "Painting", Text = "Painting" },
                new SelectListItem { Value = "Plumbing", Text = "Plumbing" }
            };
            return View(contact);
        }
        public ActionResult GenerateQRCode(string url)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    return File(memoryStream.ToArray(), "image/png");
                }
            }
        }
        public ActionResult ShowQRCode()
        {
            string urlToEncode = Url.Action("Login", "Account", null, Request.Url.Scheme);
            ViewBag.QRCodeUrl = Url.Action("GenerateQRCode", "Home", new { url = urlToEncode });
            return View();
        }

        public ActionResult WeeklyUserRegistrations()
        {
            var registrations = db.Users
                .GroupBy(u => new { Year = u.RegisteredDate.Year, Week = SqlFunctions.DatePart("wk", u.RegisteredDate) })
                .Select(g => new
                {
                    Week = g.Key.Week.ToString(),
                    Year = g.Key.Year.ToString(),
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Week)
                .ToList();

            var model = new WeeklyUserRegistrationsViewModel
            {
                Weeks = registrations.Select(r => $"{r.Year}-W{r.Week}").ToList(),
                UserCounts = registrations.Select(r => r.Count).ToList()
            };

            return View(model);
        }

    }
}
