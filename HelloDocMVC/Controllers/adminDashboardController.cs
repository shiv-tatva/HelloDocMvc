using Microsoft.AspNetCore.Mvc;

namespace HelloDocMVC.Controllers
{
    public class adminDashboardController : Controller
    {
        public IActionResult adminDashboard()
        {
            ViewBag.Admin = 1;
            return View();
        }
    }
}
