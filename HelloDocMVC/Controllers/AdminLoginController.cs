using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDocMVC.Controllers
{
    public class AdminLoginController : Controller
    {
        public IActionResult loginPage()
        {
            ViewBag.Admin = 1;
            return View();
        }
        
        public IActionResult forgotPage()
        {
            ViewBag.Admin = 1;
            return View();
        }
    }
}
