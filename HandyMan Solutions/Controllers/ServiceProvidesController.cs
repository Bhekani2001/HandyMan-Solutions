using HandyMan_Solutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class ServiceProvidesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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
                                  .Where(sq => sq.Paid =="Yes")
                                  .ToList();
            return View(service);
        }
    }
}