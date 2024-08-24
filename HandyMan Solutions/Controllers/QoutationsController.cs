using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class QoutationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult AllRequest()
        {
            var myQuotations = db.QoutationRequests.ToList();
            return View(myQuotations);
        }

        [HttpGet]
        public ActionResult MyRequest()
        {
            var userId = User.Identity.GetUserId();
            var myQuotations = db.QoutationRequests
                                  .Where(q => q.UserId == userId)
                                  .ToList();
            return View(myQuotations);
        }

        [HttpGet]
        public ActionResult QoutationHistory()
        {
            var userId = User.Identity.GetUserId();
            var myQuotations = db.QoutationRequests
                                  .Where(q => q.UserId == userId && q.Status == "Approved")
                                  .ToList();
            return View(myQuotations);
        }

        [HttpGet]
        public ActionResult SubmitRequest()
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

        [HttpPost]
        public JsonResult SubmitRequest(Qoutation qoutation, HttpPostedFileBase ImageFile)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            if (ModelState.IsValid)
            {
                qoutation.UserId = userId;
                qoutation.UserName = $"{user.FirstName} {user.LastName} {user.FamilyName}";
                qoutation.UserAddress = user.Address;
                qoutation.UserEmail = user.Email;
                qoutation.UserContact = user.Contact;
                qoutation.TechnicianAssigned = "None";
                qoutation.Status = "Pending...";
                qoutation.RequestedDate = DateTime.Now;
                qoutation.Paid = "No";
                qoutation.EstimatedCost = 0;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    if (ImageFile.ContentType == "image/jpeg" || ImageFile.ContentType == "image/png")
                    {
                        using (var binaryReader = new BinaryReader(ImageFile.InputStream))
                        {
                            qoutation.ImageData = binaryReader.ReadBytes(ImageFile.ContentLength);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Only JPEG and PNG images are allowed." });
                    }
                }

                try
                {
                    db.QoutationRequests.Add(qoutation);
                    db.SaveChanges();

                    var subject = "HandyMan Service Request";
                    var body = $"Dear {user.FirstName} {user.LastName} {user.FamilyName},<br/><br/>" +
                               $"On {DateTime.Now.ToString("MMMM dd, yyyy")}, you requested for a HandyMan Service: {qoutation.ServiceType}.<br/>" +
                               $"If it wasn't you, please ignore this email. It was intended for {user.FirstName} {user.LastName} {user.FamilyName} with email {user.Email} <br/><br/>" +
                               "Thank you,<br/>HandyMan Service Team";

                    SendEmail(user.Email, subject, body);
                    return Json(new { success = true, message = "Request submitted successfully." });
                }
                catch (Exception ex)
                {
                    // Log exception
                    return Json(new { success = false, message = "An error occurred while submitting the request." });
                }
            }

            return Json(new { success = false, message = "Please correct the errors and try again." });
        }

        private void SendEmail(string email, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(email));
                    message.From = new MailAddress("developementengineering@gmail.com");
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("developementengineering@gmail.com", "your_password");
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw;
            }
        }

        public ActionResult AssignTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Qoutation tradeIn = db.QoutationRequests.Find(id);
            if (tradeIn == null)
            {
                return HttpNotFound();
            }

            return View(tradeIn);
        }
        public ActionResult Review(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Qoutation tradeIn = db.QoutationRequests.Find(id);
            if (tradeIn == null)
            {
                return HttpNotFound();
            }

            return View(tradeIn);
        }

        [HttpPost]
        public ActionResult Approve(int qouteId)
        {
            var qoutation = db.QoutationRequests.Find(qouteId);
            if (qoutation != null && qoutation.Status != "Approved")
            {
                qoutation.Status = "Approved";
                db.SaveChanges();
                return Json(new { success = true, message = "Qoutation approved successfully!" });
            }
            return Json(new { success = false, message = "Failed to approve qoutation." });
        }

        [HttpPost]
        public ActionResult Decline(int qouteId)
        {
            var qoutation = db.QoutationRequests.Find(qouteId);
            if (qoutation != null && qoutation.Status != "Declined")
            {
                qoutation.Status = "Declined";
                db.SaveChanges();
                return Json(new { success = true, message = "Qoutation declined successfully!" });
            }
            return Json(new { success = false, message = "Failed to decline qoutation." });
        }
    }
}
