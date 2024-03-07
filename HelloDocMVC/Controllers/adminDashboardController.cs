using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HalloDoc.mvc.Auth;

namespace HelloDocMVC.Controllers
{

    [CustomAuthorize("Admin")]

    public class adminDashboardController : Controller
    {
        private IAdminDash _IAdminDash;

        public adminDashboardController(IAdminDash iAdminDash)
        {
            _IAdminDash = iAdminDash;
        }


        public IActionResult adminDashboard()
        {
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData();
            ViewBag.Admin = 1;
            return View(adminDashObj);
        }




        public IActionResult LoadPartialDashboard()
        {
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData();


            return PartialView("_adminDash", adminDashObj);
        }


        public IActionResult newViewCase(int data)
        {
            ViewBag.Admin = 4;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminDataViewCase(data);
            return View(adminDashObj);
        }


        public IActionResult newViewNote(int data)
        {
            ViewBag.Admin = 4;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj._viewNote = _IAdminDash.adminDataViewNote(data);
            return View(adminDashObj);
        }

        [HttpPost]
        public IActionResult newViewNote(adminDashData obj)
        {
            _IAdminDash.adminDataViewNote(obj);
            return RedirectToAction("newViewNote", "adminDashboard", new { data = obj._viewNote.reqid });
        }

        public IActionResult cancelCase(int req) {
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.closeCase = _IAdminDash.closeCaseNote(req);
            adminDashObj.casetagNote = _IAdminDash.casetag();
            return PartialView("_adminDashNewCancelCase", adminDashObj);
        }


        [HttpPost]
        public IActionResult cancelCase(adminDashData obj) {
            _IAdminDash.closeCaseNote(obj);
            return RedirectToAction("adminDashboard");
        }


        public IActionResult assignCase(int req)
        {
            adminDashData obj = new adminDashData();
            obj.assignCase = _IAdminDash.adminDataAssignCase(req);
            return PartialView("_adminDashNewAsignCase", obj);
        }

        public ActionResult GetDoctors(int regionId)
        {
            adminDashData obj = new adminDashData();

            obj.assignCase = _IAdminDash.adminDataAssignCaseDocList(regionId);

            return Json(new { dataid = obj.assignCase.phy_name, dataPhyId = obj.assignCase.phy_id });
        }

        [HttpPost]
        public IActionResult assignCase(adminDashData assignObj)
        {
            adminDashData obj = new adminDashData();
            _IAdminDash.adminDataAssignCase(assignObj);
            return RedirectToAction("adminDashboard");
        }


        public IActionResult blockCase(int req)
        {
            adminDashData obj = new adminDashData();
            obj._blockCaseModel = _IAdminDash.blockcase(req);

            return PartialView("_adminDashNewBlockCase", obj);
        }


        [HttpPost]
        public IActionResult blockCase(adminDashData obj)
        {
            _IAdminDash.blockcase(obj);

            return RedirectToAction("adminDashboard");
        }


        public IActionResult pendingViewUploadMain(int data)
        {
            ViewBag.Admin = 4;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj._viewUpload = _IAdminDash.viewUploadMain(data);
            return View(adminDashObj);
        }

        [HttpPost]
        public IActionResult pendingViewUploadMain(adminDashData obj)
        {
            adminDashData adminDashObj = new adminDashData();
            _IAdminDash.viewUploadMain(obj);
            return RedirectToAction("pendingViewUploadMain", "adminDashboard", new { data = obj._viewUpload[0].reqid });
        }

        public IActionResult DownloadFile(string data)
        {
            string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathname);//filepath will relative as per the filename
            string fileName = data;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, data);

        }



        public IActionResult sendMail(string email,string id,string data)
        {
            string emailMain = "20it029.shiv.santoki@vvpedulink.ac.in";
            string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
            _IAdminDash.sendMail(emailMain, pathname);
            return RedirectToAction("pendingViewUploadMain", "adminDashboard", new { data = id});

        }


        public IActionResult DeleteFile(bool data, int id, int reqFileId)
        {
            _IAdminDash.DeleteFile(data, reqFileId);
            return RedirectToAction("pendingViewUploadMain", "adminDashboard", new { data = id });

        }

        public IActionResult activeOrders(int data)
        {
            ViewBag.Admin = 4;

            adminDashData _data = new adminDashData();
            _data._activeOrder = _IAdminDash.viewOrder(data);
            return View(_data);
        }

        public IActionResult GetBusiness(int profession_id)
        {
            adminDashData adminDashData = new adminDashData();
            adminDashData._activeOrder = _IAdminDash.businessName(profession_id);

            return Json(new { business_id = adminDashData._activeOrder.business_id, business_name = adminDashData._activeOrder.business_data });
        }
        
        
        public IActionResult BusinessDetail(int business_id)
        {
            adminDashData adminDashData = new adminDashData();
            adminDashData._activeOrder = _IAdminDash.businessDetail(business_id);

            return Json(new { email = adminDashData._activeOrder.email, contact = adminDashData._activeOrder.business_contact,fax = adminDashData._activeOrder.fax_num });
        }



        [HttpPost]
        public IActionResult activeOrders(adminDashData adminDashData)
        {
            ViewBag.Admin = 4;

            adminDashData _data = new adminDashData();
            _IAdminDash.viewOrder(adminDashData);
            return RedirectToAction("activeOrders","adminDashboard",new {data = adminDashData._activeOrder.reqid});
        }
    }
}
