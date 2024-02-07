using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HelloDocMVC.DataContext;

namespace HelloDocMVC.Controllers
{
    public class SubmitRequestController : Controller
    {
        private readonly ILogger<SubmitRequestController> _logger;

        public SubmitRequestController(ILogger<SubmitRequestController> logger)
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
            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.Admins.ToList();
            return View(data);
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