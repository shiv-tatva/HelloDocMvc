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
            ViewBag.Admin = 3;
            return View();
        }

       

        private readonly IPatientRequest patientRequest;
        private readonly IConcierge conciergeRequest;
        private readonly IFamilyFriend familyFriend;
        private readonly IBusiness businessRequest;

            ApplicationDbContext db = new ApplicationDbContext();
     
        public IActionResult checkEmailAvailibility(string email) //action
        {
           
            int codefordata = patientRequest.UserExist(email) ;
            return Json(new { code = codefordata });

        }

        public PatientController(IPatientRequest patientRequest, IConcierge conciergeRequest, IFamilyFriend familyFriend, IBusiness businessRequest)
            {
                this.patientRequest = patientRequest;
                this.conciergeRequest = conciergeRequest;
                this.familyFriend = familyFriend;
                this.businessRequest = businessRequest;
            }

            public IActionResult PatientInfo()
            {
            ViewBag.Admin = 3;
            return View();
            }

            [HttpPost]
            public IActionResult PatientInfo(Custom obj)
            {
                patientRequest.userDetail(obj);
                ViewBag.Admin = 3;
                return View();
            }

        
       
   
       
        public IActionResult BusinessInfo()
        {
            ViewBag.Admin = 3;
            return View();
        }

        [HttpPost]
        public IActionResult BusinessInfo(BusinessCustome obj)
        {
            businessRequest.businessInfo(obj);
            ViewBag.Admin = 3;
            return View();
        }
        public IActionResult ConciergeInfo()
        {
            ViewBag.Admin = 3;
            return View();
        }

        [HttpPost]
        public IActionResult ConciergeInfo(ConciergeCustom obj)
        {
            conciergeRequest.ConciergeDetail(obj);
            ViewBag.Admin = 3;
            return View();
        }


        public IActionResult FamilyFriendInfo()
        {
            ViewBag.Admin = 3;
            return View();
        }

        [HttpPost]
        public IActionResult FamilyFriendInfo(FamilyFriendData data)
        {
            familyFriend.FamilyFriendInfo(data);
            ViewBag.Admin = 3;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
