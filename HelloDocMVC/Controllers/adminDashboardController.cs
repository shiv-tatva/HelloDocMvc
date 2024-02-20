using Microsoft.AspNetCore.Mvc;

namespace HelloDocMVC.Controllers
{
    public class adminDashboardController : Controller
    {
        public IActionResult adminDashboard()
        {
            return View();
        }
    }
}
