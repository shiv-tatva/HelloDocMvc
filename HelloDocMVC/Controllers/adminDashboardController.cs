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

            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

            return View();
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult LoadPartialDashboard()
        {
            int[] status = { 1 };
            adminDashData adminDashObj = new adminDashData();
            var sessionName = HttpContext.Session.GetString("UserSession");
            adminDashObj._countMain = _IAdminDash.countService(sessionName);


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


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult newViewCase(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

            ViewBag.Admin = 4;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminDataViewCase(data);
            return View(adminDashObj);
        }

        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult newViewNote(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

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


        [CustomAuthorize("Admin", "Dashboard")]

        public IActionResult pendingViewUploadMain(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

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


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult activeOrders(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

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
            TempData["success"] = "Mail Sent Successfully";
            return RedirectToAction("adminDashboard");            
        }


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult toCloseCloseCase(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

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


        [CustomAuthorize("Admin", "My Profile")]
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


        [CustomAuthorize("Admin", "Dashboard")]
        public IActionResult concludeEncounter(int data)
        {
            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;

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

        public IActionResult RegionFilter(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashNew", adminDashObj);
        }
        public IActionResult RegionFilter2(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashPending", adminDashObj);
        }
        public IActionResult RegionFilter3(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashActive", adminDashObj);
        }
        public IActionResult RegionFilter4(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashConclude", adminDashObj);
        }
        public IActionResult RegionFilter5(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashToClose", adminDashObj);
        }
        public IActionResult RegionFilter6(int[] arr, int regionId)
        {
            int typeId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            return PartialView("_adminDashUnpaid", adminDashObj);
        }

        //***************************************Provider**********************************************


        [CustomAuthorize("Admin", "Providers")]
        public IActionResult provider(int regionId)
        {
            adminDashData adminDashData = new adminDashData();
            adminDashData._provider = _IAdminDash.providerMain(regionId);
            adminDashData._RegionTable = _IAdminDash.RegionTable();
            return PartialView("_adminDashProvider", adminDashData);
        }

        public IActionResult providerContactModal(int phyId) 
        {
            adminDashData obj = new adminDashData();
            obj._provider = _IAdminDash.providerContact(phyId);

            return PartialView("_providerContactModal", obj);
        }

        [HttpPost]
        public IActionResult providerContactModalEmail(int phyIdMain,string msg)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            var _provider = _IAdminDash.providerContactEmail(phyIdMain,msg, sessionEmail);
            return RedirectToAction("provider");
        }

        public IActionResult providerCheckBox(int phyId)
        {
            var stopNotification = _IAdminDash.stopNotification(phyId);
            return Json(new {indicate = stopNotification.indicate });
        }

        public IActionResult providerEdit(int phyId)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            adminDashData data = new adminDashData();
            data._providerEdit = _IAdminDash.adminEditPhysicianProfile(phyId, sessionEmail);
            data._RegionTable = _IAdminDash.RegionTable();
            data._phyRegionTable = _IAdminDash.PhyRegionTable(phyId);
            data._role = _IAdminDash.physicainRole();
            return View(data);
        }


        [HttpPost]
        public IActionResult providerEditFirst(string password,int phyId,string email)
        {
            bool editProvider = _IAdminDash.providerResetPass(email, password);
            return Json(new { indicate = editProvider, phyId = phyId });
        }


        [HttpPost]
        public IActionResult editProviderForm1(int phyId,int roleId,int statusId)
        {
            bool editProviderForm1 = _IAdminDash.editProviderForm1(phyId,roleId, statusId);
            return Json(new { indicate = editProviderForm1, phyId = phyId });
        }

        [HttpPost]
        public IActionResult editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId,int[] phyRegionArray)
        {
            bool editProviderForm2 = _IAdminDash.editProviderForm2( fname,  lname,  email,  phone,  medical,  npi,  sync,  phyId,  phyRegionArray);
            return Json(new { indicate = editProviderForm2, phyId = phyId });
        }
        
        [HttpPost]
        public IActionResult editProviderForm3(adminDashData payloadMain)
        { 
            var editProviderForm3 = _IAdminDash.editProviderForm3(payloadMain);
            return Json(new { indicate = editProviderForm3.indicate, phyId = editProviderForm3.PhyID });
        }

        [HttpPost]
        public IActionResult PhysicianBusinessInfoEdit(adminDashData payloadMain)
        {

            var editProviderForm4 = _IAdminDash.PhysicianBusinessInfoUpdate(payloadMain);
            return Json(new { indicate = editProviderForm4.indicate, phyId = editProviderForm4.PhyID });

            //return Ok(payloadMain._providerEdit.PhyID);
        }


        [HttpPost]
        public IActionResult UpdateOnBoarding(adminDashData payloadMain)
        {
            var editProviderForm5 = _IAdminDash.EditOnBoardingData(payloadMain);
            return Json(new { indicate = editProviderForm5.indicate, phyId = editProviderForm5.PhyID });
        }

        public IActionResult editProviderDeleteAccount(int phyId)
        {
            _IAdminDash.editProviderDeleteAccount(phyId);
            return Ok();
        }


        public IActionResult createProviderAccount()
        {
            adminDashData data = new adminDashData();
            data._RegionTable = _IAdminDash.RegionTable();
            data._role = _IAdminDash.physicainRole();
            return View(data);
        }

        [HttpPost]
        public IActionResult createProviderAccount(adminDashData obj, List<int> physicianRegions)
        {
            adminDashData data = new adminDashData();
            var createProviderAccount = _IAdminDash.createProviderAccount(obj, physicianRegions);
            return Json(new { indicate = createProviderAccount.indicateTwo });
        }


        //*****************************************************Scheduling****************************************


        [CustomAuthorize("Admin", "Scheduling")]
        public IActionResult GetScheduling()
        {
            SchedulingCm schedulingCm = new SchedulingCm()
            {
                regions = _IAdminDash.RegionTable(),
            };
            return PartialView("Scheduling/_Provider_Scheduling", schedulingCm);
        }


        public IActionResult CreateNewShift()
        {
            SchedulingCm schedulingCm = new SchedulingCm();
            schedulingCm.regions = _IAdminDash.RegionTable();
            return PartialView("Scheduling/_CreateShift", schedulingCm);
        }


        public ActionResult GetRegion(int selectedregion)
        {
            var data = _IAdminDash.GetRegionvalue(selectedregion);
            return Json(data);
        }

        [HttpPost]
        public IActionResult createShiftPost(SchedulingCm schedulingCm)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
            if (_IAdminDash.createShift(schedulingCm.ScheduleModel, (int)Aspid))
            {
                return Ok(true);
            }
            return Ok(false);


        }


        [HttpPost]
        public IActionResult loadshift(string datestring, string sundaystring, string saturdaystring, string shifttype, int regionid)
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
                        monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month");
                    }
                    else
                    {
                        monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month").Where(i => i.Regionid == regionid).ToList();
                    }

                    return PartialView("Scheduling/_MonthWiseShift", monthShift);

                case "week":

                    WeekShiftModal weekShift = new WeekShiftModal();

                    weekShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                    weekShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "week");

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
                    dayShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "day");

                    return PartialView("Scheduling/_DayWiseShift", dayShift);

                default:
                    return PartialView();
            }

        }

        public IActionResult OpenScheduledModal(ViewShiftModal viewShiftModal)
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
                    var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date, DateTime.Now, DateTime.Now, "month").Where(i => i.Shiftdate.Day == viewShiftModal.columnDate.Day).ToList();
                    ViewBag.TotalShift = list.Count();
                    return PartialView("Scheduling/_MoreShift", ScheduleModel);

                default:

                    return PartialView();
            }
        }
        public IActionResult OpenScheduledModalWeek(string sundaystring, string saturdaystring, string datestring, DateTime shiftdate, int physicianid)
        {
            DateTime sunday = DateTime.Parse(sundaystring);
            DateTime saturday = DateTime.Parse(saturdaystring);

            DateTime date1 = DateTime.Parse(datestring);
            ShiftDetailsmodal ScheduleModel = new ShiftDetailsmodal();
            var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date1, sunday, saturday, "week").Where(i => i.Shiftdate.Day == shiftdate.Day && i.Physicianid == physicianid).ToList();
            ViewBag.TotalShift = list.Count();
            return PartialView("Scheduling/_MoreShift", ScheduleModel);


        }

        public IActionResult ReturnShift(int status, int shiftdetailid)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
            _IAdminDash.SetReturnShift(status, shiftdetailid, (int)Aspid);
            return Ok();
        }

        public IActionResult deleteShift(int shiftdetailid)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
            _IAdminDash.SetDeleteShift(shiftdetailid, (int)Aspid);
            return Ok();
        }

        public IActionResult EditShiftDetails(ShiftDetailsmodal shiftDetailsmodal)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");
            if (_IAdminDash.SetEditShift(shiftDetailsmodal, (int)Aspid))
            {
                return Ok(true);
            }
            return Ok(false);
        }


        //MD's On Call
        public IActionResult GetOnCall(int regionid)
        {

            var MdsCallModal = _IAdminDash.GetOnCallDetails(regionid);
            return PartialView("Scheduling/_MDs_OnCall", MdsCallModal);
        }

        public IActionResult ShiftReview(int regionId, int callId)
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

        public IActionResult ApproveShift(int[] shiftDetailsId)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");

            _IAdminDash.ApproveSelectedShift(shiftDetailsId, (int)Aspid);

            return Ok();
        }

        public IActionResult DeleteSelectedShift(int[] shiftDetailsId)
        {
            var Aspid = HttpContext.Session.GetInt32("AspNetUserID");

            _IAdminDash.DeleteShiftReview(shiftDetailsId, (int)Aspid);

            return Ok();
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
            List<Physicianlocation> getLocation = _IAdminDash.GetPhysicianlocations();
            return Ok(getLocation);
        }



        //***************************************Partners**********************************************


        [CustomAuthorize("Admin", "Partners")]
        public IActionResult partners(int professionid)
        {
            var Partnersdata = _IAdminDash.GetPartnersdata(professionid);
            partnerModel partnerModel = new partnerModel
            {
                Partnersdata = Partnersdata,
                Professions = _IAdminDash.GetProfession(),
            };

            return PartialView("_adminDashPartners", partnerModel);
        }

        
        public IActionResult AddBusiness(int vendorID)
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
        public IActionResult CreateNewBusiness(partnerModel partnerModel)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            var flag = _IAdminDash.CreateNewBusiness(partnerModel, sessionEmail);
            if (flag == true)
            {
                return Json(new { isSend = true });
            }
            return Json(new { isSend = false });
        }
        public IActionResult UpdateBusiness(partnerModel partnerModel)
        {
            if (_IAdminDash.UpdateBusiness(partnerModel))
            {
                return Json(new { Success = true, vendorid = partnerModel.vendorID });
            }
            return Json(new { Success = false, vendorid = partnerModel.vendorID });
        }
        
        
        public IActionResult DelPartner(int vendorID)
        {
            _IAdminDash.DltBusiness(vendorID);

            return Ok();
        }

        //***************************************Access**********************************************


        [CustomAuthorize("Admin", "Account Access")]
        public IActionResult access()
        {
            accessModel accessModel = new accessModel();
            accessModel.AccountAccess = _IAdminDash.GetAccountAccessData();

            return PartialView("_adminDashAccess", accessModel);
        }

        public IActionResult createAccess()
        {
            accessModel accessModelMain = new accessModel();
            accessModelMain.Menus = _IAdminDash.GetMenu(0);
            accessModelMain.Aspnetroles = _IAdminDash.GetAccountType();            
            return PartialView("_adminAccessCreateAccess", accessModelMain);
        }

        public IActionResult FilterRolesMenu(int accounttype)
        {
            var menu = _IAdminDash.GetMenu(accounttype);
            var htmlcontent = "";
            foreach (var obj in menu)
            {
                htmlcontent += $"<div class='form-check form-check-inline px-2 mx-3'><input class='form-check-input d2class' name='AccountMenu' type='checkbox' id='{obj.Menuid}' value='{obj.Menuid}'/><label class='form-check-label' for='{obj.Menuid}'>{obj.Name}</label></div>";
            }
            return Content(htmlcontent);
        }

        [HttpPost]
        public IActionResult SetCreateAccessAccount(accessModel adminAccessCm, List<int> AccountMenu)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.SetCreateAccessAccount(adminAccessCm.CreateAccountAccess, AccountMenu, sessionEmail);

            return Ok();
        }

        public IActionResult GetEditAccess(int accounttypeid, int roleid)
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

        public IActionResult FilterEditRolesMenu(int accounttypeid, int roleid)
        {
            var menu = _IAdminDash.GetAccountMenu(accounttypeid, roleid);
            var htmlcontent = "";
            foreach (var obj in menu)
            {
                htmlcontent += $"<div class='form-check form-check-inline px-2 mx-3'><input class='form-check-input d2class' {(obj.ExistsInTable ? "checked" : "")} name='AccountMenu' type='checkbox' id='{obj.menuid}' value='{obj.menuid}'/><label class='form-check-label' for='{obj.menuid}'>{obj.name}</label></div>";
            }
            return Content(htmlcontent);
        }

        [HttpPost]
        public IActionResult SetEditAccessAccount(accessModel adminAccessCm, List<int> AccountMenu)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            _IAdminDash.SetEditAccessAccount(adminAccessCm.CreateAccountAccess, AccountMenu, sessionEmail);

            return Json(new {isEdited = true});
        }

        [HttpPost]
        public IActionResult DeleteAccountAccess(int roleid)
        {
            _IAdminDash.DeleteAccountAccess(roleid);
            return Ok();
        }


        [CustomAuthorize("Admin", "User Access")]
        public IActionResult userAccess(int accounttypeid)
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


        [CustomAuthorize("Admin", "Create Admin")]
        public IActionResult createAdmin()
        {
            adminDashData data = new adminDashData();
            data._RegionTable = _IAdminDash.RegionTable();
            data._role = _IAdminDash.physicainRole();
            return View(data);
        }

        [HttpPost]
        public IActionResult createAdmin(adminDashData obj, List<int> regions)
        {
            adminDashData data = new adminDashData();   
            _IAdminDash.createAdminAccount(obj, regions);
            return Ok();
        }

        //public IActionResult adminEdit(int adminId)
        //{            
        //    adminDashData data = new adminDashData();
        //    data._providerEdit = _IAdminDash.adminEditPage(adminId);
        //    return View();
        //}

        public IActionResult adminEdit(int adminId)
        {

            adminDashData data = new adminDashData();
            data._providerEdit = _IAdminDash.adminEditPage(adminId);
            data._adminRegionTable = _IAdminDash.GetRegionsAdmin(adminId);
            data._RegionTable = _IAdminDash.RegionTable();
            data._role = _IAdminDash.physicainRole();

            return View("adminEdit", data);
        }


        [HttpPost]
        public IActionResult adminEdit(adminDashData adminDashData, List<int> adminRegions)
        {

            ViewBag.Admin = 3;

            var email = HttpContext.Session.GetString("UserSession");
            bool isaccedited = _IAdminDash.EditAdminDetailsDb(adminDashData, email, adminRegions);

            return Json(new { iseditedacc = isaccedited });
        }

        //public IActionResult providerEdit(int phyId)
        //{
        //    var sessionEmail = HttpContext.Session.GetString("UserSession");
        //    adminDashData data = new adminDashData();
        //    data._providerEdit = _IAdminDash.adminEditPhysicianProfile(phyId, sessionEmail);
        //    data._RegionTable = _IAdminDash.RegionTable();
        //    data._phyRegionTable = _IAdminDash.PhyRegionTable(phyId);
        //    data._role = _IAdminDash.physicainRole();
        //    return View(data);
        //}




        //***************************************Records**********************************************

        [CustomAuthorize("Admin", "Search Records")]
        public IActionResult searchRecords(recordsModel recordsModel)
        {
            recordsModel _data = new recordsModel();
            _data.requestListMain = _IAdminDash.searchRecords(recordsModel);
            if(_data.requestListMain.Count() == 0)
            {
                requestsRecordModel rec = new requestsRecordModel();
                rec.flag = 1;
                _data.requestListMain.Add(rec);
            }

            return PartialView("_adminDashSearchRecords", _data);
        }

        public IActionResult ScheduleExportAll(recordsModel recordsModel)
        {
            var exportAll = _IAdminDash.GenerateExcelFile(_IAdminDash.searchRecords(recordsModel));
            return File(exportAll, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
        }

        public IActionResult recordDltBtn(int reqId)
        {
           _IAdminDash.DeleteRecords(reqId);

            return Ok();
        }

        [CustomAuthorize("Admin", "Email Logs")]
        public IActionResult emailLogs(recordsModel recordsModel)
        {
            recordsModel _data = new recordsModel();
            _data = _IAdminDash.emailLogsMain(0, recordsModel);
            return PartialView("_adminDashEmailLogs",_data);
        }


        [CustomAuthorize("Admin", "SMS Logs")]
        public IActionResult smsLogs(recordsModel recordsModel)
        {
            recordsModel _data = new recordsModel();
            _data = _IAdminDash.emailLogsMain(1, recordsModel);
            return PartialView("_adminDashSmsLogs", _data);
        }



        [CustomAuthorize("Admin", "Patient Records")]
        public IActionResult records(GetRecordsModel GetRecordsModel)
        {
            GetRecordsModel _data = new GetRecordsModel();
            _data.users = _IAdminDash.patientRecords(GetRecordsModel);

            if (_data.users.Count() == 0)
            {
               _data.flag = 1;
            }

            return PartialView("_adminDashRecords", _data);
        }
        
        
        public IActionResult GetPatientRecordExplore(int userId)
        {
            recordsModel _data = new recordsModel();
            _data.getRecordExplore = _IAdminDash.GetPatientRecordExplore(userId);

            return PartialView("_adminDashRecordExplore", _data);
        }



        [CustomAuthorize("Admin", "Blocked History")]
        public IActionResult blockedHistory(recordsModel recordsModel)
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


        public IActionResult unblockBlockHistory(int blockId)
        {
           _IAdminDash.unblockBlockHistoryMain(blockId);
            return Ok();
        }
        
        //public IActionResult providerCheckBoxBlock(int blockId)
        //{
        //    var stopNotification = _IAdminDash.stopNotificationBlock(blockId);
        //    return Json(new { indicate = stopNotification.indicate });
        //}
    }
}
