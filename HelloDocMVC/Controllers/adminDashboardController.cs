using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HelloDocMVC.Controllers
{
    public class adminDashboardController : Controller
    {
        private IAdminDash _IAdminDash;

        public adminDashboardController (IAdminDash iAdminDash)
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



        
        public IActionResult LoadPartialDashboard( )
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
            return RedirectToAction("newViewNote", "adminDashboard", new {data=obj._viewNote.reqid});
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
            //adminDashData adminDashObj = new adminDashData();
            //adminDashObj.closeCase = _IAdminDash.closeCaseNote(req);
            //adminDashObj.casetagNote = _IAdminDash.casetag();
            //return PartialView("_adminDashNewAssignCase", adminDashObj);
            return PartialView("_adminDashNewAsignCase");
        }
    }
}
