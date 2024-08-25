using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class QoutationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string PayFastUrl = "https://sandbox.payfast.co.za/eng/process?";
        private const string MerchantId = "10000100";
        private const string MerchantKey = "46f0cd694581a";

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
                                  .Where(q => q.UserId == userId && q.Status == "Inspected")
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
                    // Save the quotation temporarily in session
                    Session["PendingQuotation"] = qoutation;

                    var subject = "HandyMan Service Request";
                    var body = $"Dear {user.FirstName} {user.LastName} {user.FamilyName},<br/><br/>" +
                               $"On {DateTime.Now.ToString("MMMM dd, yyyy")}, you requested a HandyMan Service: {qoutation.ServiceType}.<br/>" +
                               $"If it wasn't you, please ignore this email. It was intended for {user.FirstName} {user.LastName} {user.FamilyName} with email {user.Email}.<br/><br/>" +
                               "Thank you,<br/>HandyMan Service Team";

                    SendEmail(user.Email, subject, body);
                    return Json(new { success = true, message = "Request submitted successfully. Please proceed to payment." });
                }
                catch (Exception ex)
                {
                    // Log exception
                    return Json(new { success = false, message = "An error occurred while submitting the request." });
                }
            }

            return Json(new { success = false, message = "Please correct the errors and try again." });
        }

        [HttpPost]
        public ActionResult PayNow()
        {
            decimal quotationFee = 250m;

            // Get the pending quotation from session
            var quotation = Session["PendingQuotation"] as Qoutation;

            if (quotation == null)
            {
                return RedirectToAction("SubmitRequest");
            }

            // Create payment parameters
            var paymentParams = new Dictionary<string, string>
            {
                { "merchant_id", MerchantId },
                { "merchant_key", MerchantKey },
                { "return_url", GenerateUrl("PaymentSuccess") },
                { "cancel_url", GenerateUrl("PaymentCancelled") },
                { "notify_url", GenerateUrl("PaymentNotify") },
                { "amount", quotationFee.ToString("0.00") },
                { "item_name", "Request Payment" },
                { "email_address", User.Identity.GetUserName() }
            };

            var queryString = string.Join("&", paymentParams.Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}"));

            var payFastUrl = PayFastUrl + queryString;

            return Redirect(payFastUrl);
        }

        public ActionResult PaymentCancelled()
        {
            Session.Remove("PendingQuotation");

            ViewBag.Message = "Payment cancelled by user.";
            return View();
        }

        public ActionResult PaymentSuccess()
        {
            var quotation = Session["PendingQuotation"] as Qoutation;

            if (quotation != null)
            {
                quotation.Paid = "Yes";
                quotation.Status = "Pending...";

                db.QoutationRequests.Add(quotation);
                db.SaveChanges();

                var subject = "Payment Confirmation - HandyMan Service";
                var body = $"Dear {quotation.UserName},<br/><br/>" +
                           $"We have successfully received your payment for the HandyMan Service request: {quotation.ServiceType}.<br/>" +
                           $"Your quotation request has been approved and is now being processed.<br/><br/>" +
                           "Thank you for choosing HandyMan Services!<br/>" +
                           "HandyMan Service Team";

                SendEmail(quotation.UserEmail, subject, body);

                // Clear the session
                Session.Remove("PendingQuotation");

                ViewBag.Message = "Payment successful!";
            }
            else
            {
                ViewBag.Message = "No pending quotation found. Payment could not be processed.";
            }

            return View();
        }

        private string GenerateUrl(string actionName)
        {
            return Url.Action(actionName, "Qoutations", null, Request.Url.Scheme);
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
                        smtp.Credentials = new NetworkCredential("developementengineering@gmail.com", "dsuggidyyjvjhhvs");
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Qoutation qoutation = db.QoutationRequests.Find(id);
            if (qoutation == null)
            {
                return HttpNotFound();
            }

            return View(qoutation);
        }

        public ActionResult ProcessQoutation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Qoutation qoutation = db.QoutationRequests.Find(id);
            if (qoutation == null)
            {
                return HttpNotFound();
            }

            return View(qoutation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ProcessQoutation(Qoutation model)
        {
            if (ModelState.IsValid)
            {
                var quotation = db.QoutationRequests.FirstOrDefault(q => q.Id == model.Id);

                if (quotation != null)
                {
                    quotation.EstimatedCost = model.EstimatedCost;
                    quotation.TechnicianNotes = model.TechnicianNotes;
                    quotation.Status = "Inspected";

                    // Save changes to the database
                    await db.SaveChangesAsync();

                    // Redirect URL
                    string redirectUrl = Url.Action("QoutationDuties", "Qoutations");

                    return Json(new { success = true, message = "Quotation updated successfully.", redirectUrl = redirectUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Quotation not found." });
                }

            }
            else
            {
                return Json(new { success = false, message = "Invalid data." });
            }
        }

        public ActionResult Review(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Qoutation qoutation = db.QoutationRequests.Find(id);
            if (qoutation == null)
            {
                return HttpNotFound();
            }

            return View(qoutation);
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
            return Json(new { success = false, message = "Qoutation not found or already approved." });
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
            return Json(new { success = false, message = "Qoutation not found or already declined." });
        }

        [HttpPost]
        public ActionResult AssignTechnician(int qouteId, string technicianName)
        {
            var qoutation = db.QoutationRequests.Find(qouteId);
            if (qoutation != null && qoutation.Status == "Approved")
            {
                qoutation.TechnicianAssigned = technicianName;
                db.SaveChanges();
                return Json(new { success = true, message = "Technician assigned successfully!" });
            }
            return Json(new { success = false, message = "Qoutation not found or not approved yet." });
        }
    }
}
