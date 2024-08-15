using HandyMan_Solutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HandyMan_Solutions.Controllers
{
    public class CarOnboardingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<SelectListItem> GetYearOptions()
        {
            var years = new List<SelectListItem>();
            int startYear = 2015;
            int currentYear = DateTime.Now.Year;

            for (int year = startYear; year <= currentYear; year++)
            {
                years.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }

            return years;
        }

        public ActionResult Cars()
        {
            var tasks = db.CarOnboardings.ToList();
            return View(tasks);
        }

        [HttpGet]
        public ActionResult CarOnboarding()
        {
            ViewBag.CarTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Box Truck", Text = "Box Truck" },
                new SelectListItem { Value = "Pickup Truck", Text = "Pickup Truck" },
                new SelectListItem { Value = "Van", Text = "Van" }
            };
            
            ViewBag.CarMakeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Isuzu", Text = "Isuzu" },
                new SelectListItem { Value = "Toyota", Text = "Toyota" },
                new SelectListItem { Value = "Volkswagen", Text = "Volkswagen" }
            };

            ViewBag.YearOptions = GetYearOptions();

            return View();
        }

        [HttpPost]
        public ActionResult CarOnboarding(CarOnboarding carOnboarding)
        {
            if (ModelState.IsValid)
            {
                carOnboarding.AcquisitionDate = DateTime.Now;
                carOnboarding.CarStatus = "Available";
                db.CarOnboardings.Add(carOnboarding);
                db.SaveChanges();

                return RedirectToAction("Cars");
            }

            ViewBag.CarTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Box Truck", Text = "Box Truck" },
                new SelectListItem { Value = "Pickup Truck", Text = "Pickup Truck" },
                new SelectListItem { Value = "Van", Text = "Van" }
            };

            ViewBag.CarMakeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Isuzu", Text = "Isuzu" },
                new SelectListItem { Value = "Toyota", Text = "Toyota" },
                new SelectListItem { Value = "Volkswagen", Text = "Volkswagen" }
            };

            ViewBag.YearOptions = GetYearOptions();

            return View();
        }

        [HttpPost]
        public JsonResult DeleteCar(int id)
        {
            var carOnboarding = db.CarOnboardings.Find(id);
            if (carOnboarding != null)
            {
                db.CarOnboardings.Remove(carOnboarding);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
