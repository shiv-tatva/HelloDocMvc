using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using Microsoft.AspNetCore.Http; 

namespace HelloDocMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _login;
     
        //ApplicationDbContext db = new ApplicationDbContext();

        public LoginController(ILoginService _login)
        {
            this._login = _login;
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
            //TempData["email"] = obj.Email;
            var sessionUser  = obj.Email;
            var userName = obj.Email.Substring(0, obj.Email.IndexOf('@'));

            ViewBag.Admin = 1;

            if (data != null)
            {
                HttpContext.Session.SetString("UserSession", sessionUser);
                HttpContext.Session.SetString("UserSessionName", userName);
                return RedirectToAction("patientDashboard", "patientDashboard");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
