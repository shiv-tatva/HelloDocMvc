using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDocMVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.Admin = 1;
            return View();
        }



        private readonly IPatientDash _patientDashInfo;
        private readonly ICreateAccount createAccount;

        public HomeController(ICreateAccount createAccount, IPatientDash patientDashInfo)
        {
            this.createAccount = createAccount;
            _patientDashInfo = patientDashInfo;
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
            return RedirectToAction("CreateAccount", new { aspuserId = obj.aspnetUserId });
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


        public IActionResult pendingReviewAgreement(int reqId)
        {
            ViewBag.Admin = 1;
            PatientDashboard patientDashboardMain = new PatientDashboard();
            patientDashboardMain._reviewAgreement = _patientDashInfo.reviewAgree(reqId);
            return View(patientDashboardMain);
        }


        [HttpPost]
        public IActionResult pendingReviewAgreement(PatientDashboard obj)
        {
            if (obj._reviewAgreement.flag == 1)
            {
                if (_patientDashInfo.checkstatus(obj._reviewAgreement.reqid) == false)
                {
                    _patientDashInfo.reviewAgree(obj);
                    TempData["success"] = "Status changed Successfully!";
                }
                else
                {
                    TempData["error"] = "Status Already changed!";
                }
            }
            else
            {
                if (_patientDashInfo.checkstatus(obj._reviewAgreement.reqid) == false)
                {
                    _patientDashInfo.agreeMain(obj._reviewAgreement.reqid);
                    TempData["success"] = "Status changed Successfully!";
                }
                else
                {
                    TempData["error"] = "Status Already changed!";
                }
            }
            return RedirectToAction("pendingReviewAgreement", new { reqId = obj._reviewAgreement.reqid });

        }

    }
}