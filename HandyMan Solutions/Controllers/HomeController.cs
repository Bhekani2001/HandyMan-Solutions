using QRCoder;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using HandyMan_Solutions.Models;

namespace Tassc.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult FAQs()
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
    }
}
