using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using BLL_Business_Logic_Layer_.Services;

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

        public IActionResult CreateAccount()
        {
            ViewBag.Admin = 1;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(Aspnetuser obj) {

            ViewBag.Admin = 1;

            createAccount.createAccount(obj);

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}