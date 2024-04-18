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
using System.Reflection;
using BLL_Business_Logic_Layer_.Services;
using DAL_Data_Access_Layer_.DataModels;

namespace HelloDocMVC.Controllers
{

    //[CustomAuthorize("Admin")]
     
    public class adminDashboardController : Controller
    {
        private IAdminDash _IAdminDash;

        public adminDashboardController(IAdminDash iAdminDash)
        {
            _IAdminDash = iAdminDash;
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult adminDashboard()
        {
            try
            {
                var sessionName = HttpContext.Session.GetString("UserSessionName");
                TempData["headerUserName"] = sessionName;
                ViewBag.Admin = 4;

                var roleMain = HttpContext.Session.GetInt32("roleId");
                List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
                ViewBag.Menu = roleMenu;

                return View();
            }
            catch
            {
                return NotFound();
            }

            
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult LoadPartialDashboard()
        {
            try
            {
                int[] status = { 1 };
                adminDashData adminDashObj = new adminDashData();
                var sessionName = HttpContext.Session.GetString("UserSession");
                adminDashObj._countMain = _IAdminDash.countService(sessionName, 0);

                return PartialView("_adminDash", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult newTab(int[] arr)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();

                return PartialView("_adminDashNew", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

          
        }

        public IActionResult pendingTab(int[] arr)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("_adminDashPending", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult activeTab(int[] arr)
        {
            try
            {

                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("_adminDashActive", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

        }

        public IActionResult concludeTab(int[] arr)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("_adminDashConclude", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult toCloseTab(int[] arr)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("_adminDashToClose", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        public IActionResult unpaidTab(int[] arr)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("_adminDashUnpaid", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult tableRecords(int[] arr,int typeId)
        {
            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashNew", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult tableRecords2(int[] arr,int typeId)
        {
            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashPending", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }
        public IActionResult tableRecords3(int[] arr,int typeId)
        {
            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashActive", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult tableRecords4(int[] arr,int typeId)
        {
            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashConclude", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult tableRecords5(int[] arr,int typeId)
        {

            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashToClose", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult tableRecords6(int[] arr,int typeId)
        {
            try
            {
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashUnpaid", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult newViewCase(int data,int flag)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminDataViewCase(data, flag);
                return View(adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult newViewNote(int data)
        {

            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj._viewNote = _IAdminDash.adminDataViewNote(data);

                return View(adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        [HttpPost]
        public IActionResult newViewNote(adminDashData obj)
        {
            try
            {
                _IAdminDash.adminDataViewNote(obj);
                return Json(new { data = obj._viewNote.reqid });
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult cancelCase(int req) {

            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.closeCase = _IAdminDash.closeCaseNote(req);
                adminDashObj.casetagNote = _IAdminDash.casetag();
                return PartialView("_adminDashNewCancelCase", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }

        [HttpPost]
        public IActionResult cancelCase(adminDashData obj) {
            try
            {
                _IAdminDash.closeCaseNote(obj);
                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }

          
        }

        public IActionResult assignCase(int req)
        {

            try
            {
                adminDashData obj = new adminDashData();
                obj.assignCase = _IAdminDash.adminDataAssignCase(req);
                return PartialView("_adminDashNewAsignCase", obj);
            }
            catch
            {
                return NotFound();
            }

           
        }

        public ActionResult GetDoctors(int regionId)
        {

            try
            {
                adminDashData obj = new adminDashData();

                obj.assignCase = _IAdminDash.adminDataAssignCaseDocList(regionId);

                return Json(new { dataid = obj.assignCase.phy_name, dataPhyId = obj.assignCase.phy_id });
            }
            catch
            {
                return NotFound();
            }

           
        }


        [HttpPost]
        public IActionResult assignCase(adminDashData assignObj)
        {
            try
            {
                adminDashData obj = new adminDashData();
                _IAdminDash.adminDataAssignCase(assignObj);
                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult blockCase(int req)
        {

            try
            {
                adminDashData obj = new adminDashData();
                obj._blockCaseModel = _IAdminDash.blockcase(req);

                return PartialView("_adminDashNewBlockCase", obj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        [HttpPost]
        public IActionResult blockCase(adminDashData obj)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.blockcase(obj, sessionEmail);

                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }


           
        }


        [CustomAuthorize("Admin", "Dashboard")]

        public IActionResult pendingViewUploadMain(int data,int flag)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj._viewUpload = _IAdminDash.viewUploadMain(data, flag);
                return View(adminDashObj);
            }
            catch
            {
                return NotFound();
            }


           
        }
          

        [HttpPost]
        public IActionResult pendingViewUploadMain(adminDashData obj)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                _IAdminDash.viewUploadMain(obj);
                return Json(new { data = obj._viewUpload[0].reqid });
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult DownloadFile(string data)
        {
            try
            {
                string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathname);//filepath will relative as per the filename
                string fileName = data;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, data);
            }
            catch
            {
                return NotFound();
            }

            

        }

        public IActionResult sendMail(string email,string id,string[] data)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                //string emailMain = "20it029.shiv.santoki@vvpedulink.ac.in";
                //string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
                _IAdminDash.sendMail(email, data, sessionEmail);
                return RedirectToAction("pendingViewUploadMain", "adminDashboard", new { data = id });
            }
            catch
            {
                return NotFound();
            }

           

        }


        [HttpPost]
        public IActionResult DeleteFile(bool data, int id, int reqFileId)
        {
            try
            {
                _IAdminDash.DeleteFile(data, reqFileId);
                return Json(new { data = id });
            }
            catch
            {
                return NotFound();
            }

            

        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult activeOrders(int data)
        {
            try
            {
                adminDashData _data = new adminDashData();
                _data._activeOrder = _IAdminDash.viewOrder(data);
                return View(_data);
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult GetBusiness(int profession_id)
        {
            try
            {
                adminDashData adminDashData = new adminDashData();
                adminDashData._activeOrder = _IAdminDash.businessName(profession_id);

                return Json(new { business_id = adminDashData._activeOrder.business_id, business_name = adminDashData._activeOrder.business_data });
            }
            catch
            {
                return NotFound();
            }


           
        }
        
        
        public IActionResult BusinessDetail(int business_id)
        {
            try
            {
                adminDashData adminDashData = new adminDashData();
                adminDashData._activeOrder = _IAdminDash.businessDetail(business_id);

                return Json(new { email = adminDashData._activeOrder.email, contact = adminDashData._activeOrder.business_contact, fax = adminDashData._activeOrder.fax_num });
            }
            catch
            {
                return NotFound();
            }

            
        }



        [HttpPost]
        public IActionResult activeOrders(adminDashData adminDashData)
        {
            try
            {
                adminDashData _data = new adminDashData();
                _IAdminDash.viewOrder(adminDashData);
                return Json(new { data = adminDashData._activeOrder.reqid });
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult transferCase(int req)
        {
            try
            {
                adminDashData obj = new adminDashData();
                obj.transferRequest = _IAdminDash.transferReq(req);
                return PartialView("_adminDashPendingTransfer", obj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        [HttpPost]
        public IActionResult transferCase(adminDashData data)
        {

            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.transferReq(data, sessionEmail);
                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult clearCase(int req)
        {

            try
            {
                adminDashData obj = new adminDashData();
                obj._blockCaseModel = _IAdminDash.clearCase(req);
                return PartialView("_adminDashPendingClearCase", obj);
            }
            catch
            {
                return NotFound();
            }

            
        }


        [HttpPost]
        public IActionResult clearCase(adminDashData block)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.clearCase(block, sessionEmail);
                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }

           
        }


        public IActionResult sendAgreement(int req)
        {
            try
            {
                adminDashData obj = new adminDashData();
                obj._sendAgreement = _IAdminDash.sendAgree(req);
                return PartialView("_adminPendingSendAgreement", obj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        [HttpPost]
        public IActionResult sendAgreement(adminDashData dataMain)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.sendAgree(dataMain, sessionEmail);
                TempData["success"] = "Mail Sent Successfully";
                return RedirectToAction("adminDashboard");
            }
            catch
            {
                return NotFound();
            }

                       
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult toCloseCloseCase(int data)
        {
            try
            {
                adminDashData obj = new adminDashData();
                obj._closeCaseMain = _IAdminDash.closeCaseMain(data);
                return View(obj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult DeleteFileTwo(bool data, int id, int reqFileId)
        {
            try
            {
                _IAdminDash.DeleteFile(data, reqFileId);
                return RedirectToAction("toCloseCloseCase", "adminDashboard", new { data = id });
            }
            catch
            {
                return NotFound();
            }

            

        }

        [HttpPost]
        public IActionResult closeCaseBtn(adminDashData obj)
        {
            try
            {
                _IAdminDash.closeCaseSaveMain(obj);
                return Json(new { data = obj._closeCaseMain.reqid });
            }
            catch
            {
                return NotFound();
            }

            
        }


        
        public IActionResult closeCaseCloseBtn(int data)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.closeCaseCloseBtn(data, sessionEmail);
                return RedirectToAction("adminDashboard", new { req = data });
            }
            catch
            {
                return NotFound();
            }

            
        }


        [CustomAuthorize("Admin", "My Profile")]
        public IActionResult myProfile()
        {
            try
            {
                adminDashData _admin = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _admin._myProfile = _IAdminDash.myProfile(sessionEmail);
                _admin._adminRegionTable = _IAdminDash.GetRegionsAdmin((int)_admin._myProfile.admin_id);
                _admin._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminMyProfile", _admin);
            }
            catch
            {
                return NotFound();
            }

            
        }


        [HttpPost]
        public IActionResult myProfile(adminDashData obj, List<int> adminRegions)
        {

            try
            {
                if (obj._myProfile.flag == 1)
                {
                    var sessionEmail = HttpContext.Session.GetString("UserSession");
                    bool isSend = _IAdminDash.myProfileReset(obj._myProfile, sessionEmail);
                    return Json(new { isSend = isSend });
                }
                else if (obj._myProfile.flag == 2)
                {
                    var sessionEmail = HttpContext.Session.GetString("UserSession");
                    var isSend = _IAdminDash.myProfileAdminInfo(obj._myProfile, sessionEmail, adminRegions);

                    var userName = isSend.email.Substring(0, isSend.email.IndexOf('@'));
                    if (isSend.email != null)
                    {

                        TempData["usernameMyProfile"] = userName;
                        HttpContext.Session.SetString("UserSessionName", userName);
                        HttpContext.Session.SetString("UserSession", isSend.email);
                    }
                    return Json(new { isSend = isSend.indicate, userChangeHeader = userName });
                }
                else
                {
                    var sessionEmail = HttpContext.Session.GetString("UserSession");
                    bool isSend = _IAdminDash.myProfileAdminBillingInfo(obj._myProfile, sessionEmail);
                    return Json(new { isSend = isSend });

                }
            }
            catch
            {
                return NotFound();
            }

           
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult concludeEncounter(int data)
        {
            try
            {
                adminDashData _admin = new adminDashData();
                _admin._encounter = _IAdminDash.concludeEncounter(data);
                return View(_admin);
            }
            catch
            {
                return NotFound();
            }

           
        }

        [HttpPost]
        public IActionResult concludeEncounter(concludeEncounter encounter)
        {

            try
            {
                var isSend = _IAdminDash.concludeEncounter(encounter);
                return Json(new { isSend = isSend.indicate });
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult sendLinkPopUp()
        {
            return PartialView("_sendLink");
        }


        [HttpPost]
        public IActionResult sendLinkPopUp(sendLink data)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var isSend = _IAdminDash.sendLink(data, sessionEmail);
                return Json(new { isSend = isSend.indicate });
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult createRequest()
        {
            return PartialView("_createRequest");
        }

        public IActionResult verifyState(string stateMain)
        {
            try
            {
                var isSend = _IAdminDash.verifyState(stateMain);
                return Json(new { isSend = isSend.indicate });
            }
            catch
            {
                return NotFound();
            }

           
        }

        [HttpPost]
        public IActionResult createRequestMain(createRequest data)
        {
            try
            {
                int flag = 0;
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var isSend = _IAdminDash.createRequest(data, sessionEmail, flag);
                return Json(new { createReq = isSend.indicate });
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult requestSupportPopUp()
        {
            return PartialView("_requestSupport");
        }

        public IActionResult Export(string GridHtml)
        {

            try
            {
                return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "ExportData.xls");
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult ExportAll()
        {

            try
            {
                int regionId = 0;
                int typeId = 0;
                int[] status = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                var exportAll = _IAdminDash.GenerateExcelFile(_IAdminDash.adminData(status, typeId, regionId, null, 0));
                return File(exportAll, "application/vnd.ms-excel", "ExportAll.xls");
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult RegionFilter(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashNew", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult RegionFilter2(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashPending", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult RegionFilter3(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashActive", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult RegionFilter4(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashConclude", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult RegionFilter5(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashToClose", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }
        public IActionResult RegionFilter6(int[] arr, int regionId)
        {

            try
            {
                int typeId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashUnpaid", adminDashObj);
            }
            catch
            {
                return NotFound();
            }
            
        }

        //***************************************Provider**********************************************


        [CustomAuthorize("Admin", "Providers")]
        public IActionResult provider(int regionId)
        {

            try
            {
                adminDashData adminDashData = new adminDashData();
                adminDashData._provider = _IAdminDash.providerMain(regionId);
                adminDashData._RegionTable = _IAdminDash.RegionTable();
                return PartialView("_adminDashProvider", adminDashData);
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult providerContactModal(int phyId) 
        {

            try
            {
                adminDashData obj = new adminDashData();
                obj._provider = _IAdminDash.providerContact(phyId);

                return PartialView("_providerContactModal", obj);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult providerContactModalEmail(int phyIdMain,string msg)
        {

            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var _provider = _IAdminDash.providerContactEmail(phyIdMain, msg, sessionEmail);
                return RedirectToAction("provider");
            }
            catch
            {
                return NotFound();
            }

            
        }
        
        
        [HttpPost]
        public IActionResult providerContactModalSms(int phyIdMain,string msg)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var _provider = _IAdminDash.providerContactSms(phyIdMain, msg, sessionEmail);
                return RedirectToAction("provider");
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult providerCheckBox(int phyId)
        {
            try
            {
                var stopNotification = _IAdminDash.stopNotification(phyId);
                return Json(new { indicate = stopNotification.indicate });
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult providerEdit(int phyId,int flag)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashData data = new adminDashData();
                data._providerEdit = _IAdminDash.adminEditPhysicianProfile(phyId, sessionEmail, flag, 0);
                data._RegionTable = _IAdminDash.RegionTable();
                data._phyRegionTable = _IAdminDash.PhyRegionTable(phyId);
                data._role = _IAdminDash.physicainRole();
                return View(data);
            }
            catch
            {
                return NotFound();
            }

            
        }


        [HttpPost]
        public IActionResult providerEditFirst(string password,int phyId,string email)
        {
            try
            {
                bool editProvider = _IAdminDash.providerResetPass(email, password);
                return Json(new { indicate = editProvider, phyId = phyId });
            }
            catch
            {
                return NotFound();
            }

           
        }


        [HttpPost]
        public IActionResult editProviderForm1(int phyId,int roleId,int statusId)
        {
            try
            {
                bool editProviderForm1 = _IAdminDash.editProviderForm1(phyId, roleId, statusId);
                return Json(new { indicate = editProviderForm1, phyId = phyId });
            }
            catch
            {
                return NotFound();
            }

            
        }

        [HttpPost]
        public IActionResult editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId,int[] phyRegionArray)
        {
            try
            {
                bool editProviderForm2 = _IAdminDash.editProviderForm2(fname, lname, email, phone, medical, npi, sync, phyId, phyRegionArray);
                return Json(new { indicate = editProviderForm2, phyId = phyId });
            }
            catch
            {
                return NotFound();
            }

           
        }
        
        [HttpPost]
        public IActionResult editProviderForm3(adminDashData payloadMain)
        {

            try
            {
                var editProviderForm3 = _IAdminDash.editProviderForm3(payloadMain);
                return Json(new { indicate = editProviderForm3.indicate, phyId = editProviderForm3.PhyID });
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult PhysicianBusinessInfoEdit(adminDashData payloadMain)
        {
            try
            {
                var editProviderForm4 = _IAdminDash.PhysicianBusinessInfoUpdate(payloadMain);
                return Json(new { indicate = editProviderForm4.indicate, phyId = editProviderForm4.PhyID });
            }
            catch
            {
                return NotFound();
            }

           

            //return Ok(payloadMain._providerEdit.PhyID);
        }


        [HttpPost]
        public IActionResult UpdateOnBoarding(adminDashData payloadMain)
        {

            try
            {
                var editProviderForm5 = _IAdminDash.EditOnBoardingData(payloadMain);
                return Json(new { indicate = editProviderForm5.indicate, phyId = editProviderForm5.PhyID });
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult editProviderDeleteAccount(int phyId)
        {
            try
            {
                _IAdminDash.editProviderDeleteAccount(phyId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

          
        }


        public IActionResult createProviderAccount()
        {

            try
            {
                adminDashData data = new adminDashData();
                data._RegionTable = _IAdminDash.RegionTable();
                data._role = _IAdminDash.physicainRole();
                return View(data);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult createProviderAccount(adminDashData obj, List<int> physicianRegions)
        {

            try
            {
                adminDashData data = new adminDashData();
                var createProviderAccount = _IAdminDash.createProviderAccount(obj, physicianRegions);
                return Json(new { indicate = createProviderAccount.indicateTwo });
            }
            catch
            {
                return NotFound();
            }
           
        }


        //*****************************************************Scheduling****************************************


        [CustomAuthorize("Admin", "Scheduling")]
        public IActionResult GetScheduling()
        {

            try
            {
                SchedulingCm schedulingCm = new SchedulingCm()
                {
                    regions = _IAdminDash.RegionTable(),
                };
                return PartialView("Scheduling/_Provider_Scheduling", schedulingCm);
            }
            catch
            {
                return NotFound();
            }
           
        }


        public IActionResult CreateNewShift()
        {

            try
            {
                SchedulingCm schedulingCm = new SchedulingCm();
                schedulingCm.regions = _IAdminDash.RegionTable();
                return PartialView("Scheduling/_CreateShift", schedulingCm);
            }
            catch
            {
                return NotFound();
            }
           
        }


        public ActionResult GetRegion(int selectedregion)
        {
            try
            {
                var data = _IAdminDash.GetRegionvalue(selectedregion);
                return Json(data);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult createShiftPost(SchedulingCm schedulingCm)
        {
            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
                if (_IAdminDash.createShift(schedulingCm.ScheduleModel, (int)Aspid))
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch
            {
                return NotFound();
            }
            


        }


        [HttpPost]
        public IActionResult loadshift(string datestring, string sundaystring, string saturdaystring, string shifttype, int regionid)
        {
            try
            {
                DateTime date = DateTime.Parse(datestring);
                DateTime sunday = DateTime.Parse(sundaystring);
                DateTime saturday = DateTime.Parse(saturdaystring);


                switch (shifttype)
                {
                    case "month":
                        MonthShiftModal monthShift = new MonthShiftModal();

                        var totalDays = DateTime.DaysInMonth(date.Year, date.Month);
                        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                        var startDayIndex = (int)firstDayOfMonth.DayOfWeek;

                        var dayceiling = (int)Math.Ceiling((float)(totalDays + startDayIndex) / 7);

                        monthShift.daysLoop = (int)dayceiling * 7;
                        monthShift.daysInMonth = totalDays;
                        monthShift.firstDayOfMonth = firstDayOfMonth;
                        monthShift.startDayIndex = startDayIndex;
                        monthShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                        if (regionid == 0)
                        {
                            monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month", 0, "");
                        }
                        else
                        {
                            monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month", 0, "").Where(i => i.Regionid == regionid).ToList();
                        }

                        return PartialView("Scheduling/_MonthWiseShift", monthShift);

                    case "week":

                        WeekShiftModal weekShift = new WeekShiftModal();

                        weekShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                        weekShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "week", 0, "");

                        List<int> dlist = new List<int>();

                        for (var i = 0; i < 7; i++)
                        {
                            var date12 = sunday.AddDays(i);
                            dlist.Add(date12.Day);
                        }

                        weekShift.datelist = dlist.ToList();
                        weekShift.dayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

                        return PartialView("Scheduling/_WeekWiseShift", weekShift);

                    case "day":

                        DayShiftModal dayShift = new DayShiftModal();
                        dayShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                        dayShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "day", 0, "");

                        return PartialView("Scheduling/_DayWiseShift", dayShift);

                    default:
                        return PartialView();
                }
            }
            catch
            {
                return NotFound();
            }

           

        }

        public IActionResult OpenScheduledModal(ViewShiftModal viewShiftModal)
        {
            try
            {
                HttpContext.Session.SetInt32("shiftdetailsid", viewShiftModal.shiftdetailsid);
                switch (viewShiftModal.actionType)
                {
                    case "shiftdetails":
                        ShiftDetailsmodal shift = _IAdminDash.GetShift(viewShiftModal.shiftdetailsid);
                        return PartialView("Scheduling/_ViewShift", shift);

                    case "moremonthshifts":
                        DateTime date = DateTime.Parse(viewShiftModal.datestring);
                        ShiftDetailsmodal ScheduleModel = new ShiftDetailsmodal();
                        var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date, DateTime.Now, DateTime.Now, "month", 0, "").Where(i => i.Shiftdate.Day == viewShiftModal.columnDate.Day).ToList();
                        ViewBag.TotalShift = list.Count();
                        return PartialView("Scheduling/_MoreShift", ScheduleModel);

                    default:

                        return PartialView();
                }
            }
            catch
            {
                return NotFound();
            }

           
        }
        public IActionResult OpenScheduledModalWeek(string sundaystring, string saturdaystring, string datestring, DateTime shiftdate, int physicianid)
        {
            try
            {
                DateTime sunday = DateTime.Parse(sundaystring);
                DateTime saturday = DateTime.Parse(saturdaystring);

                DateTime date1 = DateTime.Parse(datestring);
                ShiftDetailsmodal ScheduleModel = new ShiftDetailsmodal();
                var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date1, sunday, saturday, "week", 0, "").Where(i => i.Shiftdate.Day == shiftdate.Day && i.Physicianid == physicianid).ToList();
                ViewBag.TotalShift = list.Count();
                return PartialView("Scheduling/_MoreShift", ScheduleModel);
            }
            catch
            {
                return NotFound();
            }

           


        }

        public IActionResult ReturnShift(int status, int shiftdetailid)
        {
            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
                _IAdminDash.SetReturnShift(status, shiftdetailid, (int)Aspid);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult deleteShift(int shiftdetailid)
        {

            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
                _IAdminDash.SetDeleteShift(shiftdetailid, (int)Aspid);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult EditShiftDetails(ShiftDetailsmodal shiftDetailsmodal)
        {

            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
                if (_IAdminDash.SetEditShift(shiftDetailsmodal, (int)Aspid))
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch
            {
                return NotFound();
            }
            
        }


        //MD's On Call
        public IActionResult GetOnCall(int regionid)
        {
            try
            {
                var MdsCallModal = _IAdminDash.GetOnCallDetails(regionid);
                return PartialView("Scheduling/_MDs_OnCall", MdsCallModal);
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult ShiftReview(int regionId, int callId)
        {
            try
            {
                SchedulingCm schedulingCm = new SchedulingCm()
                {
                    regions = _IAdminDash.RegionTable(),
                    ShiftReview = _IAdminDash.GetShiftReview(regionId, callId),
                    regionId = regionId,
                    callId = callId,
                };

                return PartialView("Scheduling/_ShiftReview", schedulingCm);
            }
            catch
            {
                return NotFound();
            }
           
        }

        public IActionResult ApproveShift(int[] shiftDetailsId)
        {
            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");

                _IAdminDash.ApproveSelectedShift(shiftDetailsId, (int)Aspid);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult DeleteSelectedShift(int[] shiftDetailsId)
        {
            try
            {
                var Aspid = HttpContext.Session.GetInt32("AspNetUserID");

                _IAdminDash.DeleteShiftReview(shiftDetailsId, (int)Aspid);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }






        //***************************************Provider Location**********************************************

        [CustomAuthorize("Admin", "Provider Location")]
        public IActionResult providerLocation()
        {            
            return PartialView("_adminDashProviderLocation");
        }


        [HttpGet]
        public IActionResult GetProviderLocation()
        {
            try
            {
                List<Physicianlocation> getLocation = _IAdminDash.GetPhysicianlocations();
                return Ok(getLocation);
            }
            catch
            {
                return NotFound();
            }
           
        }



        //***************************************Partners**********************************************


        [CustomAuthorize("Admin", "Partners")]
        public IActionResult partners(int professionid)
        {
            try
            {
                var Partnersdata = _IAdminDash.GetPartnersdata(professionid);
                partnerModel partnerModel = new partnerModel
                {
                    Partnersdata = Partnersdata,
                    Professions = _IAdminDash.GetProfession(),
                };

                return PartialView("_adminDashPartners", partnerModel);
            }
            catch
            {
                return NotFound();
            }
           
        }

        
        public IActionResult AddBusiness(int vendorID)
        {
            try
            {
                if (vendorID == 0)
                {
                    partnerModel partnerModel = new partnerModel
                    {
                        Professions = _IAdminDash.GetProfession(),
                        regions = _IAdminDash.RegionTable(),
                        vendorID = vendorID,
                    };
                    return PartialView("_adminDashPartnersBusiness", partnerModel);
                }
                else
                {
                    partnerModel partnerModel = _IAdminDash.GetEditBusinessData(vendorID);
                    partnerModel.Professions = _IAdminDash.GetProfession();
                    partnerModel.regions = _IAdminDash.RegionTable();
                    partnerModel.vendorID = vendorID;
                    return PartialView("_adminDashPartnersBusiness", partnerModel);
                }
            }
            catch
            {
                return NotFound();
            }
           

        }
        public IActionResult CreateNewBusiness(partnerModel partnerModel)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var flag = _IAdminDash.CreateNewBusiness(partnerModel, sessionEmail);
                if (flag == true)
                {
                    return Json(new { isSend = true });
                }
                return Json(new { isSend = false });
            }
            catch
            {
                return NotFound();
            }
           
        }
        public IActionResult UpdateBusiness(partnerModel partnerModel)
        {
            try
            {
                if (_IAdminDash.UpdateBusiness(partnerModel))
                {
                    return Json(new { Success = true, vendorid = partnerModel.vendorID });
                }
                return Json(new { Success = false, vendorid = partnerModel.vendorID });
            }
            catch
            {
                return NotFound();
            }

            
        }
        
        
        public IActionResult DelPartner(int vendorID)
        {

            try
            {
                _IAdminDash.DltBusiness(vendorID);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
           
        }

        //***************************************Access**********************************************


        [CustomAuthorize("Admin", "Account Access")]
        public IActionResult access()
        {
            try
            {
                accessModel accessModel = new accessModel();
                accessModel.AccountAccess = _IAdminDash.GetAccountAccessData();

                return PartialView("_adminDashAccess", accessModel);
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult createAccess()
        {
            try
            {
                accessModel accessModelMain = new accessModel();
                accessModelMain.Menus = _IAdminDash.GetMenu(0);
                accessModelMain.Aspnetroles = _IAdminDash.GetAccountType();
                return PartialView("_adminAccessCreateAccess", accessModelMain);
            }
            catch
            {
                return NotFound();
            }
           
        }

        public IActionResult FilterRolesMenu(int accounttype)
        {
            try
            {
                var menu = _IAdminDash.GetMenu(accounttype);
                var htmlcontent = "";
                foreach (var obj in menu)
                {
                    htmlcontent += $"<div class='form-check form-check-inline px-2 mx-3'><input class='form-check-input d2class' name='AccountMenu' type='checkbox' id='{obj.Menuid}' value='{obj.Menuid}'/><label class='form-check-label' for='{obj.Menuid}'>{obj.Name}</label></div>";
                }
                return Content(htmlcontent);
            }
            catch
            {
                return NotFound();
            }
           
        }

        [HttpPost]
        public IActionResult SetCreateAccessAccount(accessModel adminAccessCm, List<int> AccountMenu)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.SetCreateAccessAccount(adminAccessCm.CreateAccountAccess, AccountMenu, sessionEmail);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
           
        }

        public IActionResult GetEditAccess(int accounttypeid, int roleid)
        {
            try
            {
                var roledata = _IAdminDash.GetEditAccessData(roleid);
                var Accounttype = _IAdminDash.GetAccountType();
                var menu = _IAdminDash.GetAccountMenu(accounttypeid, roleid);
                accessModel adminAccessCm = new accessModel
                {
                    Aspnetroles = Accounttype,
                    AccountMenu = menu,
                    CreateAccountAccess = roledata,
                };
                return PartialView("_adminAccessEdit", adminAccessCm);
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult FilterEditRolesMenu(int accounttypeid, int roleid)
        {
            try
            {
                var menu = _IAdminDash.GetAccountMenu(accounttypeid, roleid);
                var htmlcontent = "";
                foreach (var obj in menu)
                {
                    htmlcontent += $"<div class='form-check form-check-inline px-2 mx-3'><input class='form-check-input d2class' {(obj.ExistsInTable ? "checked" : "")} name='AccountMenu' type='checkbox' id='{obj.menuid}' value='{obj.menuid}'/><label class='form-check-label' for='{obj.menuid}'>{obj.name}</label></div>";
                }
                return Content(htmlcontent);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult SetEditAccessAccount(accessModel adminAccessCm, List<int> AccountMenu)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IAdminDash.SetEditAccessAccount(adminAccessCm.CreateAccountAccess, AccountMenu, sessionEmail);

                return Json(new { isEdited = true });
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult DeleteAccountAccess(int roleid)
        {
            try
            {
                _IAdminDash.DeleteAccountAccess(roleid);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }


        [CustomAuthorize("Admin", "User Access")]
        public IActionResult userAccess(int accounttypeid)
        {
            try
            {
                var userdata = _IAdminDash.GetUserdata(accounttypeid);
                var Accounttype = _IAdminDash.GetAccountTypeRoles();
                accessModel adminAccessCm = new accessModel
                {
                    UserAccess = userdata,
                    AspnetUserroles = Accounttype,
                };

                return PartialView("_adminDashUserAccess", adminAccessCm);
            }
            catch
            {
                return NotFound();
            }
            
        }


        [CustomAuthorize("Admin", "Create Admin")]
        public IActionResult createAdmin()
        {
            try
            {
                adminDashData data = new adminDashData();
                data._RegionTable = _IAdminDash.RegionTable();
                data._role = _IAdminDash.adminRole();
                return View(data);
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public IActionResult createAdmin(adminDashData obj, List<int> regions)
        {
            try
            {
                adminDashData data = new adminDashData();
                _IAdminDash.createAdminAccount(obj, regions);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }

        //public IActionResult adminEdit(int adminId)
        //{            
        //    adminDashData data = new adminDashData();
        //    data._providerEdit = _IAdminDash.adminEditPage(adminId);
        //    return View();
        //}

        public IActionResult adminEdit(int adminId)
        {
            try
            {
                adminDashData data = new adminDashData();
                data._providerEdit = _IAdminDash.adminEditPage(adminId);
                data._adminRegionTable = _IAdminDash.GetRegionsAdmin(adminId);
                data._RegionTable = _IAdminDash.RegionTable();
                data._role = _IAdminDash.adminRole();

                return View("adminEdit", data);
            }
            catch
            {
                return NotFound();
            }
            

        }


        [HttpPost]
        public IActionResult adminEdit(adminDashData adminDashData, List<int> adminRegions)
        {
            try
            {
                ViewBag.Admin = 3;

                var email = HttpContext.Session.GetString("UserSession");
                bool isaccedited = _IAdminDash.EditAdminDetailsDb(adminDashData, email, adminRegions);

                return Json(new { iseditedacc = isaccedited });
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult editDeleteAdminAccount(int adminId)
        {
            try
            {
                _IAdminDash.editDeleteAdminAccount(adminId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
        }




        //***************************************Records**********************************************

        [CustomAuthorize("Admin", "Search Records")]
        public IActionResult searchRecords(recordsModel recordsModel)
        {
            try
            {
                recordsModel _data = new recordsModel();
                _data.requestListMain = _IAdminDash.searchRecords(recordsModel);
                if (_data.requestListMain.Count() == 0)
                {
                    requestsRecordModel rec = new requestsRecordModel();
                    rec.flag = 1;
                    _data.requestListMain.Add(rec);
                }

                return PartialView("_adminDashSearchRecords", _data);
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult ScheduleExportAll(recordsModel recordsModel)
        {
            try
            {
                var exportAll = _IAdminDash.GenerateExcelFile(_IAdminDash.searchRecords(recordsModel));
                return File(exportAll, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult recordDltBtn(int reqId)
        {
            try
            {
                _IAdminDash.DeleteRecords(reqId);

                return Ok();
            }
            catch
            {
                return NotFound();
            }

           
        }

        [CustomAuthorize("Admin", "Email Logs")]
        public IActionResult emailLogs(recordsModel recordsModel)
        {
            try
            {
                recordsModel _data = new recordsModel();
                _data = _IAdminDash.emailLogsMain(0, recordsModel);
                return PartialView("_adminDashEmailLogs", _data);
            }
            catch
            {
                return NotFound();
            }


           
        }


        [CustomAuthorize("Admin", "SMS Logs")]
        public IActionResult smsLogs(recordsModel recordsModel)
        {

            try
            {
                recordsModel _data = new recordsModel();
                _data = _IAdminDash.emailLogsMain(1, recordsModel);
                return PartialView("_adminDashSmsLogs", _data);
            }
            catch
            {
                return NotFound();
            }

           
        }



        [CustomAuthorize("Admin", "Patient Records")]
        public IActionResult records(GetRecordsModel GetRecordsModel)
        {

            try
            {
                GetRecordsModel _data = new GetRecordsModel();
                _data.users = _IAdminDash.patientRecords(GetRecordsModel);

                if (_data.users.Count() == 0)
                {
                    _data.flag = 1;
                }

                return PartialView("_adminDashRecords", _data);
            }
            catch
            {
                return NotFound();
            }

           
        }
        
        
        public IActionResult GetPatientRecordExplore(int userId)
        {


            try
            {
                recordsModel _data = new recordsModel();
                _data.getRecordExplore = _IAdminDash.GetPatientRecordExplore(userId);

                return PartialView("_adminDashRecordExplore", _data);
            }
            catch
            {
                return NotFound();
            }
            
        }



        [CustomAuthorize("Admin", "Blocked History")]
        public IActionResult blockedHistory(recordsModel recordsModel)
        {

            try
            {
                recordsModel _data = new recordsModel();
                _data.blockHistoryMain = _IAdminDash.blockHistory(recordsModel);
                if (_data.blockHistoryMain.Count() == 0)
                {
                    blockHistory obj = new blockHistory();
                    obj.flag = 1;
                    _data.blockHistoryMain.Add(obj);
                }
                return PartialView("_adminDashBlockedHistory", _data);
            }
            catch
            {
                return NotFound();
            }


           
        }


        public IActionResult unblockBlockHistory(int blockId)
        {
            try
            {
                _IAdminDash.unblockBlockHistoryMain(blockId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }
        
    }
}
