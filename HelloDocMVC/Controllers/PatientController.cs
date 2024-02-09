using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using HelloDocMvc.CustomeModel.Custome;

namespace HelloDocMVC.Controllers
{
    public class PatientController : Controller
    {

        public IActionResult SubmitRequest()
        {
            return View();
        }

        private readonly IPatientRequest patientRequest;

        ApplicationDbContext db = new ApplicationDbContext();

        public PatientController(IPatientRequest patientRequest)
        {
            this.patientRequest = patientRequest;
        }

        public IActionResult PatientInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientInfo(Custome obj)
        {
            patientRequest.userDetail(obj);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
