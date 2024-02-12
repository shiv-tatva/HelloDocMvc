using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.CustomeModel;

namespace HelloDocMVC.Controllers
{
    public class PatientController : Controller
    {

        public IActionResult SubmitRequest()
        {
            return View();
        }

       

        private readonly IPatientRequest patientRequest;
        private readonly IConcierge conciergeRequest;

            ApplicationDbContext db = new ApplicationDbContext();
     
        public IActionResult checkEmailAvailibility(string email) //action
        {
           
            int codefordata = patientRequest.UserExist(email) ;
            return Json(new { code = codefordata });

        }

        public PatientController(IPatientRequest patientRequest, IConcierge conciergeRequest)
            {
                this.patientRequest = patientRequest;
                this.conciergeRequest = conciergeRequest;
            }

            public IActionResult PatientInfo()
            {
                return View();
            }

            [HttpPost]
            public IActionResult PatientInfo(Custom obj)
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

        [HttpPost]
        public IActionResult ConciergeInfo(ConciergeCustom obj)
        {
            conciergeRequest.ConciergeDetail(obj);

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
