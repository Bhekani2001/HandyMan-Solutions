using HandyMan_Solutions.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class PaymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string PayFastUrl = "https://sandbox.payfast.co.za/eng/process?";
        private const string MerchantId = "10000100";
        private const string MerchantKey = "46f0cd694581a";

        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }

        // POST: PayNow
        [HttpPost]
        public ActionResult PayNow(int quotationId)
        {
            var quotation = db.QoutationRequests.Find(quotationId);

            if (quotation == null)
            {
                return HttpNotFound("Quotation not found.");
            }

            decimal quotationFee = quotation.EstimatedCost;

            var paymentParams = new Dictionary<string, string>
            {
                { "merchant_id", MerchantId },
                { "merchant_key", MerchantKey },
                { "return_url", GenerateUrl("PaymentSuccess", new { quotationId }) },
                { "cancel_url", GenerateUrl("PaymentCancelled") },
                { "notify_url", GenerateUrl("PaymentNotify") },
                { "amount", quotationFee.ToString("0.00") },
                { "item_name", "Service Payment" },
                { "email_address", User.Identity.GetUserName() }
            };

            var queryString = string.Join("&", paymentParams.Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}"));

            var payFastUrl = PayFastUrl + queryString;

            return Redirect(payFastUrl);
        }

        public ActionResult PaymentCancelled()
        {
            ViewBag.Message = "Payment cancelled by user.";
            return View();
        }

        public ActionResult PaymentSuccess(int quotationId)
        {
            var quotation = db.QoutationRequests.Find(quotationId);
            if (quotation == null)
            {
                return HttpNotFound("Quotation not found.");
            }

            var serviceProvided = new ServiceProvided
            {
                UserName = quotation.UserName,
                UserAddress = quotation.UserAddress,
                UserContact = quotation.UserContact,
                UserEmail = quotation.UserEmail,
                ServiceType = quotation.ServiceType,
                Paid = "Yes",
                PaymentDate = DateTime.Now,
                Status = "Paid",
                TechnicianAssigned = quotation.TechnicianAssigned,
            };

            db.ServiceProvideds.Add(serviceProvided);
            db.SaveChanges();

            quotation.NewPaid = "Paid";
            db.Entry(quotation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "Payment successful!";
            return View();
        }

        private string GenerateUrl(string actionName, object routeValues = null)
        {
            return Url.Action(actionName, "Payments", routeValues, Request.Url.Scheme);
        }
    }
}
