using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HalloDoc.mvc.Auth;
using System.Text;

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
            var sessionName = HttpContext.Session.GetString("UserSessionName");
            TempData["headerUserName"] = sessionName;
            ViewBag.Admin = 4;
            return View();
        }


        public IActionResult LoadPartialDashboard()
        {
            int[] status = { 1 };
            adminDashData adminDashObj = new adminDashData();
            adminDashObj._countMain = _IAdminDash.countService();
            return PartialView("_adminDash", adminDashObj);
        }
        
        
        public IActionResult newTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            return PartialView("_adminDashNew", adminDashObj);
        }
        public IActionResult pendingTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId,regionId);

            return PartialView("_adminDashPending", adminDashObj);
        }
        public IActionResult activeTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            return PartialView("_adminDashActive", adminDashObj);
        }
        public IActionResult concludeTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            return PartialView("_adminDashConclude", adminDashObj);
        }
        public IActionResult toCloseTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            return PartialView("_adminDashToClose", adminDashObj);
        }
        
        public IActionResult unpaidTab(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            return PartialView("_adminDashUnpaid", adminDashObj);
        }

        public IActionResult tableRecords(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashNew", adminDashObj);
        }
        public IActionResult tableRecords2(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashPending", adminDashObj);
        }
        public IActionResult tableRecords3(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashActive", adminDashObj);
        }
        public IActionResult tableRecords4(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashConclude", adminDashObj);
        }
        public IActionResult tableRecords5(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashToClose", adminDashObj);
        }
        public IActionResult tableRecords6(int[] arr,int typeId)
        {
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr,typeId, regionId);
            return PartialView("_adminDashUnpaid", adminDashObj);
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
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.blockcase(obj,sessionEmail);

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



        public IActionResult sendMail(string email,string id,string[] data)
        {

            //string emailMain = "20it029.shiv.santoki@vvpedulink.ac.in";
            //string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
            _IAdminDash.sendMail(email, data);
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
            adminDashData _data = new adminDashData();
            _IAdminDash.viewOrder(adminDashData);
            return RedirectToAction("activeOrders","adminDashboard",new {data = adminDashData._activeOrder.reqid});
        }

        public IActionResult transferCase(int req)
        {
            adminDashData obj = new adminDashData();
            obj.transferRequest = _IAdminDash.transferReq(req);
            return PartialView("_adminDashPendingTransfer",obj);
        }

        [HttpPost]
        public IActionResult transferCase(adminDashData data)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.transferReq(data, sessionEmail);
            return RedirectToAction("adminDashboard");
        }


        public IActionResult clearCase(int req)
        {
            adminDashData obj = new adminDashData();
            obj._blockCaseModel = _IAdminDash.clearCase(req);
            return PartialView("_adminDashPendingClearCase",obj);
        }


        [HttpPost]
        public IActionResult clearCase(adminDashData block)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.clearCase(block, sessionEmail);
            return RedirectToAction("adminDashboard");
        }


        public IActionResult sendAgreement(int req)
        {
            adminDashData obj = new adminDashData();
            obj._sendAgreement = _IAdminDash.sendAgree(req);
            return PartialView("_adminPendingSendAgreement", obj);
        }


        [HttpPost]
        public IActionResult sendAgreement(adminDashData dataMain)
        {
            _IAdminDash.sendAgree(dataMain);
            return RedirectToAction("adminDashboard");
        }


        public IActionResult toCloseCloseCase(int data)
        {
            ViewBag.Admin = 4;
            adminDashData obj = new adminDashData();
            obj._closeCaseMain = _IAdminDash.closeCaseMain(data);
            return View(obj);
        }

        public IActionResult DeleteFileTwo(bool data, int id, int reqFileId)
        {
            _IAdminDash.DeleteFile(data, reqFileId);
            return RedirectToAction("toCloseCloseCase", "adminDashboard", new { data = id });

        }

        [HttpPost]
        public IActionResult closeCaseBtn(adminDashData obj)
        {
            _IAdminDash.closeCaseSaveMain(obj);
            return RedirectToAction("toCloseCloseCase", new { data = obj._closeCaseMain.reqid });
        }


        
        public IActionResult closeCaseCloseBtn(int data)
        {

            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.closeCaseCloseBtn(data, sessionEmail);
            return RedirectToAction("adminDashboard", new { req = data });
        }

        public IActionResult myProfile()
        {
            adminDashData _admin = new adminDashData();
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _admin._myProfile = _IAdminDash.myProfile( sessionEmail);
            return PartialView("_adminMyProfile", _admin);
        }


        [HttpPost]
        public IActionResult myProfile(myProfile obj)
        {

            if(obj.flag == 1)
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                bool isSend = _IAdminDash.myProfileReset(obj, sessionEmail);
                return Json(new { isSend = isSend});                
            }
            else if(obj.flag == 2)
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var isSend = _IAdminDash.myProfileAdminInfo(obj, sessionEmail);

                var userName = isSend.email.Substring(0, isSend.email.IndexOf('@'));
                if (isSend.email != null)
                {
                    
                    TempData["usernameMyProfile"] = userName;
                    HttpContext.Session.SetString("UserSessionName", userName);
                    HttpContext.Session.SetString("UserSession", isSend.email);
                }
                return Json(new { isSend = isSend.indicate , userChangeHeader = userName });
            }
            else
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                bool isSend = _IAdminDash.myProfileAdminBillingInfo(obj, sessionEmail);
                return Json(new { isSend = isSend });
                
            }
        }

        public IActionResult concludeEncounter(int data)
        {
            ViewBag.Admin = 4;
            adminDashData _admin = new adminDashData();
            _admin._encounter = _IAdminDash.concludeEncounter(data);
            return View(_admin);
        }

        [HttpPost]
        public IActionResult concludeEncounter(concludeEncounter encounter)
        {
            var isSend = _IAdminDash.concludeEncounter(encounter);
            return Json(new {isSend = isSend.indicate});
        }

        public IActionResult sendLinkPopUp()
        {
            return PartialView("_sendLink");
        }


        [HttpPost]
        public IActionResult sendLinkPopUp(sendLink data)
        {
            var isSend = _IAdminDash.sendLink(data);
            return Json(new {isSend = isSend.indicate});
        }

        public IActionResult createRequest()
        {
            return PartialView("_createRequest");
        }

        public IActionResult verifyState(string stateMain)
        {
            var isSend = _IAdminDash.verifyState(stateMain);
            return Json(new { isSend = isSend.indicate });
        }

        [HttpPost]
        public IActionResult createRequestMain(createRequest data)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            var isSend = _IAdminDash.createRequest(data, sessionEmail);
            return Json(new { createReq = isSend.indicate });
        }


        public IActionResult requestSupportPopUp()
        {
            return PartialView("_requestSupport");
        }

        public IActionResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "ExportData.xls");
        }

        public IActionResult ExportAll()
        {
            int regionId = 0;
            int typeId = 0;
            int[] status = { 1 ,2,3,4,5,6,7,8,9};
            var exportAll = _IAdminDash.GenerateExcelFile(_IAdminDash.adminData(status, typeId, regionId));
            return File(exportAll, "application/vnd.ms-excel", "ExportAll.xls");
        }

        public IActionResult RegionFilter(int[] arr,int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);

            if(adminDashObj.data.Count == 0)
            {
                adminDashObj.data[0].status = arr[0];
            }
            return PartialView("_adminDashNew", adminDashObj);
        }
    }
}
