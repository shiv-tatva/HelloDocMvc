using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;

namespace HelloDocMVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.Admin = 1;
            return View();
        }

        private readonly ICreateAccount createAccount;

        public HomeController(ICreateAccount createAccount)
        {
            this.createAccount = createAccount;
        }

        public IActionResult CreateAccount(int aspuserId)
        {

            ViewBag.Admin = 1;
            var acc = createAccount.createMain(aspuserId);
            return View(acc);
        }

        [HttpPost]
        public IActionResult CreateAccount(createAcc obj) 
        {
            TempData["success"] = "Account Created Successfully!";
            createAccount.createAccount(obj);
            return RedirectToAction("CreateAccount", new { aspuserId = obj.aspnetUserId});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult logoutSession()
        {
            TempData["success"] = "Logout Successfully!";
            HttpContext.Session.Clear();
            Response.Cookies.Delete("jwt");
            return RedirectToAction("LoginPage", "Login");
        }

    }
}