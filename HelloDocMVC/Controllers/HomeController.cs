using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HelloDocMVC.DataContext;
using HelloDocMVC.DataModels;

namespace HelloDocMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ApplicationDbContext db = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Admin = 1;
            return View();
        }
        public IActionResult LoginPage()
        {
            ViewBag.Admin = 1;
            return View();
        }

        [HttpPost]
        public IActionResult LoginPage(string Email, string Passwordhash)
        {
           


            var data = db.Aspnetusers.FirstOrDefault(x => x.Email == Email && x.Passwordhash == Passwordhash);

            ViewBag.Admin = 1;

            if (data != null)
            {
                return Content("hello");
            }
            else
            {
                ViewBag.LoginMessage = "Can't Login";
            }

            
            return View();

        }
        public IActionResult ForgotPage()
        {
            ViewBag.Admin = 1;
            return View();
        }

        public IActionResult PatientInfo()
        {
            return View();
        }
        public IActionResult BusinessInfo()
        {
            return View();
        }
        public IActionResult ConciergeInfo()
        {
            return View();
        } 
        public IActionResult FamilyFriendInfo()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            
            var data = db.Aspnetroles.ToList();
            return View(data);
        }
        public IActionResult Privacy2()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.Aspnetroles.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Aspnetrole a)
        {
            if(ModelState.IsValid == true)
            {
                db.Aspnetroles.Add(a);
                int b = db.SaveChanges();
                if (b > 0)
                {
                    TempData["InsertMessage"] = "<script>alert('Data Inserted')</script>";
                    return RedirectToAction("Privacy2", "Home");
                }
                else
                {
                    ViewBag.InsertMessage = "<script>alert('Data Not Inserted')</script>";
                }
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            var row = db.Aspnetroles.Where(model => model.Id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Aspnetrole a)
        {
            db.Entry(a).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            int b = db.SaveChanges();

            if(b > 0)
            {
                TempData["UpdateMessage"] = "<script>alert('Data Updated')</script>";
                return RedirectToAction("Privacy2", "Home");
            }
            else
            {
                ViewBag.UpdateMessage = "<script>alert('Data Not Updated')</script>";
            }

            return View();
        }
        public IActionResult Delete(string id)
        {
            var row = db.Aspnetroles.Where(model => model.Id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public IActionResult Delete(Aspnetrole a)
        {
            db.Entry(a).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            int b = db.SaveChanges();

            if(b > 0)
            {
                TempData["DeleteMessage"] = "<script>alert('Data Deleted')</script>";
                return RedirectToAction("Privacy2", "Home");
            }
            else
            {
                ViewBag.DeleteMessage = "<script>alert('Data Not Deleted')</script>";
            }

            return View();
        }


        public IActionResult SubmitRequest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}