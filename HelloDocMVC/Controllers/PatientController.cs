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
            try
            {
                ViewBag.Admin = 3;
                return View();
            }
            catch
            {
                return NotFound();
            }
           
        }

       

        private readonly IPatientRequest patientRequest;
        private readonly IConcierge conciergeRequest;
        private readonly IFamilyFriend familyFriend;
        private readonly IBusiness businessRequest;
        private IAdminDash _IAdminDash;


        ApplicationDbContext db = new ApplicationDbContext();
     
        public IActionResult checkEmailAvailibility(string email) //action
        {
            try
            {
                int codefordata = patientRequest.UserExist(email);
                return Json(new { code = codefordata });
            }
            catch
            {
                return NotFound();
            }
           

        }

        public PatientController(IPatientRequest patientRequest, IConcierge conciergeRequest, IFamilyFriend familyFriend, IBusiness businessRequest, IAdminDash iAdminDash)
            {
                this.patientRequest = patientRequest;
                this.conciergeRequest = conciergeRequest;
                this.familyFriend = familyFriend;
                this.businessRequest = businessRequest;
                _IAdminDash = iAdminDash;
        }

        public IActionResult PatientInfo()
        {
            try
            {
                ViewBag.Admin = 3;
                Custom _Custom = new Custom();
                _Custom._RegionTable = _IAdminDash.RegionTable();
                return View(_Custom);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult PatientInfo(Custom obj)
        {
            try
            {
                TempData["success"] = "Request Submitted Successfully!";
                patientRequest.userDetail(obj);
                return RedirectToAction("PatientInfo");
            }
            catch
            {
                return NotFound();
            }
            
        }





        public IActionResult BusinessInfo()
        {
            try
            {
                ViewBag.Admin = 3;
                BusinessCustome _business = new BusinessCustome();
                _business._RegionTable = _IAdminDash.RegionTable();
                return View(_business);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult BusinessInfo(BusinessCustome obj)
        {
            try
            {
                TempData["success"] = "Request Submitted Successfully!";
                businessRequest.businessInfo(obj);
                return RedirectToAction("BusinessInfo");
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult ConciergeInfo()
        {
            try
            {
                ViewBag.Admin = 3;
                ConciergeCustom _cons = new ConciergeCustom();
                _cons._RegionTable = _IAdminDash.RegionTable();
                return View(_cons);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult ConciergeInfo(ConciergeCustom obj)
        {
            try
            {
                TempData["success"] = "Request Submitted Successfully!";
                conciergeRequest.ConciergeDetail(obj);
                return RedirectToAction("ConciergeInfo");
            }
            catch
            {
                return NotFound();
            }
           
        }


        public IActionResult FamilyFriendInfo()
        {
            try
            {
                ViewBag.Admin = 3;
                FamilyFriendData _family = new FamilyFriendData();
                _family._RegionTable = _IAdminDash.RegionTable();
                return View(_family);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult FamilyFriendInfo(FamilyFriendData data)
        {
            try
            {
                TempData["success"] = "Request Submitted Successfully!";
                familyFriend.FamilyFriendInfo(data);
                return RedirectToAction("FamilyFriendInfo");
            }
            catch
            {
                return NotFound();
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
