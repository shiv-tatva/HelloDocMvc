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
using DAL_Data_Access_Layer_.DataContext;
using Rotativa.AspNetCore;

namespace HelloDocMVC.Controllers
{
    //[CustomAuthorize("Provider")]
    public class ProviderController : Controller
    {
        private IProviderDash _IProviderDash;
        private IAdminDash _IAdminDash;

        public ProviderController (IProviderDash IProviderDash,IAdminDash IAdminDash)
        {
            _IProviderDash = IProviderDash;
            _IAdminDash = IAdminDash;
        }


        [CustomAuthorize("Provider", "Dashboard")]
        public IActionResult Provider()
        {

            try
            {
                var sessionName = HttpContext.Session.GetString("UserSessionName");
                TempData["headerUserName"] = sessionName;
                ViewBag.Admin = 5;

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



        //*************************************************Dashboard****************************************************************

        [CustomAuthorize("Provider", "Dashboard")]
        public IActionResult LoadPartialDashboard()
        {
            try
            {
                int[] status = { 1 };
                adminDashData adminDashObj = new adminDashData();
                var sessionName = HttpContext.Session.GetString("UserSession");
                adminDashObj._countMain = _IAdminDash.countService(sessionName, 10);

                return PartialView("Provider/_ProviderDashboard", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult newTabTwo(int[] arr, int dataFlag)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, sessionEmail, dataFlag,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();

                return PartialView("Provider/_ProviderNewTab", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

        }

        public IActionResult tableRecords(int[] arr, int typeId)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0, sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("Provider/_ProviderNewTab", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult tableRecords2(int[] arr, int typeId)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("Provider/_ProviderTabPending", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }
        public IActionResult tableRecords3(int[] arr, int typeId)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("Provider/_ProviderTabActive", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult tableRecords4(int[] arr, int typeId)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, null, 0,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();
                return PartialView("Provider/_ProviderTabConclude", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }


        [HttpPost]
        public IActionResult ProivderAccept(int reqId)
        {
            try
            {
                _IProviderDash.ProivderAccept(reqId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult newViewCase(int data, int flag)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj.data = _IAdminDash.adminDataViewCase(data, flag);
                return PartialView("Provider/_ProviderViewCase", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

         
        }


        public IActionResult newViewNote(int data)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj._viewNote = _IAdminDash.adminDataViewNote(data);
                return PartialView("Provider/_ProviderViewNote", adminDashObj);
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
                _IProviderDash.physicianDataViewNote(obj);
                return Json(new { data = obj._viewNote.reqid });
            }
            catch
            {
                return NotFound();
            }

          
        }

        public IActionResult pendingViewUploadMain(int data, int flag)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj._viewUpload = _IAdminDash.viewUploadMain(data, flag);
                return PartialView("Provider/_ProviderViewUpload", adminDashObj);
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

        public IActionResult sendMail(string email, string id, string[] data)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                //string emailMain = "20it029.shiv.santoki@vvpedulink.ac.in";
                //string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
                _IAdminDash.sendMail(email, data, sessionEmail);
                return RedirectToAction("pendingViewUploadMain", "Provider", new { data = id });
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


        public IActionResult pendingTabTwo(int[] arr, int dataFlag)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, sessionEmail, dataFlag,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();

                return PartialView("Provider/_ProviderTabPending", adminDashObj);
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
                return PartialView("Provider/_ProviderSendAgreement", obj);
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
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }

        //provider Transfer request
        public IActionResult ProviderTransferRequest(int requestid)
        {

            try
            {
                var adminDashData = _IProviderDash.ProviderTransferMain(requestid);

                return PartialView("Provider/_ProviderTransfer", adminDashData);
            }
            catch
            {
                return NotFound();
            }

           
        }


        public IActionResult PostTransferRequest(adminDashData adminDashData)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IProviderDash.PostTransferRequest(adminDashData._ProviderTransferTab.Note, (int)adminDashData._ProviderTransferTab.reqId, sessionEmail);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult activeOrders(int data)
        {
            try
            {
                adminDashData _data = new adminDashData();
                _data._activeOrder = _IAdminDash.viewOrder(data);
                return PartialView("Provider/_ProviderOrder", _data);
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
        
        



        public IActionResult activeTabTwo(int[] arr, int dataFlag)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, sessionEmail, dataFlag,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("Provider/_ProviderTabActive", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

           
        }


        public IActionResult ProviderEncounterPopUp(int data)
        {
            try
            {
                adminDashData adminDashData = new adminDashData();
                adminDashData = _IProviderDash.ProviderEncounterPopUp(data);
                return PartialView("Provider/_ProviderEncounterModal", adminDashData);
            }
            catch
            {
                return NotFound();
            }

           
        }

        [HttpPost]
        public IActionResult ProviderEncounterPopUp(adminDashData data)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IProviderDash.ProviderEncounterPopUp(data, sessionEmail);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }

        public IActionResult HousecallPopUp(int reqId)
        {
            try
            {
                adminDashData adminDashData = new adminDashData();
                adminDashData = _IProviderDash.HousecallPopUp(reqId);
                return PartialView("Provider/_ProviderHouseCall", adminDashData);
            }
            catch
            {
                return NotFound();
            }

            
        }
        
        
        public IActionResult HouseCallConclude(int reqId)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IProviderDash.HouseCallConclude(reqId, sessionEmail);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult concludeEncounter(int data)
        {
            try
            {
                adminDashData _admin = new adminDashData();
                _admin._encounter = _IAdminDash.concludeEncounter(data);
                return PartialView("Provider/_ProviderEncounterForm", _admin);
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
                return Json(new { isSend = isSend.indicate, data = encounter.reqid });
            }
            catch
            {
                return NotFound();
            }

          
        }
        
        public IActionResult FinalizeEncounter(int reqId)
        {
            try
            {
              var indicate =  _IProviderDash.FinalizeEncounter(reqId);
                return Json(new {isSend = indicate });
            }
            catch
            {
                return NotFound();
            }

           
        }
        
        
        public IActionResult ProviderEncounterFormDownload(int data)
        {
            try
            {
                var dataMain = _IProviderDash.ProviderEncounterFormDownload(data);
                return Json(new { isSend = dataMain._ProviderEncounterPopUp.reqId });
            }
            catch
            {
                return NotFound();
            }

           
        }
        
        
        public IActionResult FinalizePopup(int data)
        {
            try
            {
                var dataMain = _IProviderDash.ProviderEncounterFormDownload(data);
                return Json(new { isSend = dataMain._ProviderEncounterPopUp.reqId });
            }
            catch
            {
                return NotFound();
            }

           
        }

        public IActionResult GeneratePDF(int requestid)
        {
            try
            {
                adminDashData model = new adminDashData();
                model._encounter = _IAdminDash.concludeEncounter(requestid);

                if (model == null)
                {
                    TempData["error"] = "Something Went Wrong!";
                    return NotFound();
                }

                DateTime currentDateTime = DateTime.Now;
                string fileName = $"encounter_{currentDateTime.ToString("ddMMyyyy_HHmmss")}.pdf";
                TempData["success"] = "ENCOUTER FORM Pdf is generated!";
                return new ViewAsPdf("Provider/_ProviderEncounterPdf", model)
                {
                    FileName = fileName
                };
            }
            catch
            {
                return NotFound();
            }

           

        }


        public IActionResult concludeTabTwo(int[] arr, int dataFlag)
        {
            try
            {
                int typeId = 0;
                int regionId = 0;
                adminDashData adminDashObj = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId, sessionEmail, dataFlag,sessionEmail);
                adminDashObj._RegionTable = _IAdminDash.RegionTable();


                return PartialView("Provider/_ProviderTabConclude", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult ConcludeCare(int data, int flag)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                adminDashObj._viewUpload = _IAdminDash.viewUploadMain(data, flag);
                return PartialView("Provider/_ProviderConcludeCare", adminDashObj);
            }
            catch
            {
                return NotFound();
            }

            
        }


        [HttpPost]
        public IActionResult ConcludeViewUploadMain(adminDashData obj)
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
        
        
        
        [HttpPost]
        public IActionResult ProviderConcludeCarePost(adminDashData obj)
        {
            try
            {
                adminDashData adminDashObj = new adminDashData();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IProviderDash.ProviderConcludeCarePost(obj, sessionEmail);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }


        public IActionResult sendLinkPopUp()
        {
            return PartialView("Provider/_ProviderSendLink");
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
            return PartialView("Provider/_ProviderCreateRequest");
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
                int flag = 15;
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                var isSend = _IAdminDash.createRequest(data, sessionEmail, flag);
                return Json(new { createReq = isSend.indicate });
            }
            catch
            {
                return NotFound();
            }

            
        }

        //*****************************************************My Profile******************************************************************************



        [CustomAuthorize("Provider", "My Profile")]
        public IActionResult myProfile(int statusId, int flag)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                adminDashData data = new adminDashData();
                data._providerEdit = _IAdminDash.adminEditPhysicianProfile(0, sessionEmail, flag, statusId);
                data._RegionTable = _IAdminDash.RegionTable();
                data._phyRegionTable = _IAdminDash.PhyRegionTable(data._providerEdit.PhyID);
                data._role = _IAdminDash.physicainRole();
                return PartialView("Provider/_ProviderMyProfile", data);
            }
            catch
            {
                return NotFound();
            }

            
        }

        [HttpPost]
        public IActionResult providerEditFirst(string password, int phyId, string email)
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
        
        
        public IActionResult RequestAdmin()
        {
            return PartialView("Provider/_ProviderRequestAdmin");
        }

        [HttpPost]
        public IActionResult RequestAdmin(ProviderTransferTab _ProviderTransferTab)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                _IProviderDash.RequestAdmin(_ProviderTransferTab, sessionEmail);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

            
        }


        //*****************************************************Scheduling******************************************************************************


        [CustomAuthorize("Provider", "My Schedule")]
        public IActionResult GetScheduling()
        {
            return PartialView("Provider/_ProviderSchedule");
        }


        public IActionResult CreateNewShift()
        {
            try
            {
                SchedulingCm schedulingCm = new SchedulingCm();
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                schedulingCm = _IProviderDash.PhysicainRegionTable(sessionEmail);
                return PartialView("Provider/_ProviderScheduleCreateShift", schedulingCm);
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
                var sessionEmail = HttpContext.Session.GetString("UserSession");

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
                            monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month", 15, sessionEmail);
                        }
                        else
                        {
                            monthShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "month", 15, sessionEmail).Where(i => i.Regionid == regionid).ToList();
                        }

                        return PartialView("Provider/_ProviderScheduleMonthWise", monthShift);

                    case "week":

                        WeekShiftModal weekShift = new WeekShiftModal();

                        weekShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                        weekShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "week", 15, sessionEmail);

                        List<int> dlist = new List<int>();

                        for (var i = 0; i < 7; i++)
                        {
                            var date12 = sunday.AddDays(i);
                            dlist.Add(date12.Day);
                        }

                        weekShift.datelist = dlist.ToList();
                        weekShift.dayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

                        return PartialView("Provider/_WeekWiseShift", weekShift);

                    case "day":

                        DayShiftModal dayShift = new DayShiftModal();
                        dayShift.Physicians = _IAdminDash.GetPhysicians(regionid);
                        dayShift.shiftDetailsmodals = _IAdminDash.ShiftDetailsmodal(date, sunday, saturday, "day", 15, sessionEmail);

                        return PartialView("Provider/_DayWiseShift", dayShift);

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
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                HttpContext.Session.SetInt32("shiftdetailsid", viewShiftModal.shiftdetailsid);
                switch (viewShiftModal.actionType)
                {
                    case "shiftdetails":
                        ShiftDetailsmodal shift = _IAdminDash.GetShift(viewShiftModal.shiftdetailsid);
                        return PartialView("Provider/_ProviderScheduleViewShift", shift);

                    case "moremonthshifts":
                        DateTime date = DateTime.Parse(viewShiftModal.datestring);
                        ShiftDetailsmodal ScheduleModel = new ShiftDetailsmodal();
                        var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date, DateTime.Now, DateTime.Now, "month", 15, sessionEmail).Where(i => i.Shiftdate.Day == viewShiftModal.columnDate.Day).ToList();
                        ViewBag.TotalShift = list.Count();
                        return PartialView("Provider/_ProviderScheduleMoreShift", ScheduleModel);

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
                var sessionEmail = HttpContext.Session.GetString("UserSession");
                DateTime sunday = DateTime.Parse(sundaystring);
                DateTime saturday = DateTime.Parse(saturdaystring);

                DateTime date1 = DateTime.Parse(datestring);
                ShiftDetailsmodal ScheduleModel = new ShiftDetailsmodal();
                var list = ScheduleModel.ViewAllList = _IAdminDash.ShiftDetailsmodal(date1, sunday, saturday, "week", 15, sessionEmail).Where(i => i.Shiftdate.Day == shiftdate.Day && i.Physicianid == physicianid).ToList();
                ViewBag.TotalShift = list.Count();
                return PartialView("Provider/_ProviderScheduleMoreShift", ScheduleModel);
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
                return PartialView("Provider/_MDs_OnCall", MdsCallModal);
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

                return PartialView("Provider/_ShiftReview", schedulingCm);
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

    }
}
