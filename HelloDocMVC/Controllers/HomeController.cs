using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
//using HelloDocMVC.DataContext;
//using HelloDocMVC.DataModels;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;

namespace HelloDocMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILoginService _login;

        ApplicationDbContext db = new ApplicationDbContext();

        public HomeController(ILoginService _login )
        {
          this._login = _login;
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
        public IActionResult LoginPage(Aspnetuser obj)
        {

            var data = _login.login(obj);

            ViewBag.Admin = 1;

            if (data != null)
            {
                return RedirectToAction("Privacy2");
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

        //[HttpPost]
        //public IActionResult PatientInfo(User obj)
        //{
        //    patientRequest.userDetail(obj);

        //    return View();
        //}
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
            if (ModelState.IsValid == true)
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