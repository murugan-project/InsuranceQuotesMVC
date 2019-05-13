using InsuranceQuotesMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InsuranceQuotesMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetQuote(string FirstName, string LastName, string EmailAddress, string DOB, string CarMake, string CarYear, string CarModel, string SpeedingTickets, string DUI, string coverage)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(DOB) || string.IsNullOrEmpty(CarMake) || string.IsNullOrEmpty(CarYear) || string.IsNullOrEmpty(CarModel) || string.IsNullOrEmpty(SpeedingTickets) || string.IsNullOrEmpty(DUI) || string.IsNullOrEmpty(coverage))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (var db = new ApplicantsEntities1())
                {

                    var user = new user();

                    //Assigning entered info into DB
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.EmailAddress = EmailAddress;
                    user.DOB = Convert.ToDateTime(DOB);
                    user.CarMake = CarMake;
                    user.CarYear = Convert.ToInt32(CarYear);
                    user.CarModel = CarModel;
                    user.SpeedingTickets = Convert.ToInt16(SpeedingTickets);
                    if (DUI == "Yes")
                    {
                        user.DUI = true;
                    }
                    else
                    {
                        user.DUI = false;
                    }
                    if (coverage == "FullCoverage")
                    {
                        user.FullCoverage = true;
                    }
                    // Quote Calculation
                    double quote = 50;
                    int age = new DateTime(DateTime.Now.Subtract(Convert.ToDateTime(user.DOB)).Ticks).Year - 1;
                    if (age < 25 && age > 18 || age > 100)
                    {
                        quote += 25;
                    }
                    else if (age < 18)
                    {
                        quote += 100;

                    }

                    if (user.CarYear < 2000 || user.CarYear > 2015)
                    {
                        quote += 25;
                    }

                    if (user.CarMake == "Porsche")
                    {
                        quote += 25;
                    }
                    else if (user.CarMake == "Porsche" && user.CarModel == "911 Carrera")
                    {
                        quote += 25;
                    }

                    quote += Convert.ToInt32(user.SpeedingTickets) * 10;

                    if (Convert.ToBoolean(user.DUI))
                    {
                        quote += quote * 0.25;
                    }

                    if (Convert.ToBoolean(user.FullCoverage))
                    {
                        quote += quote * 0.5;
                    }
                    user.Quote = quote;
                    db.users.Add(user);
                    db.SaveChanges();

                    return View(user);

                }
            }
        }




        public ActionResult Admin()
        {
            using (ApplicantsEntities1 db = new ApplicantsEntities1())
            {
                return View(db.users.ToList());
            }
        }

    }
}
