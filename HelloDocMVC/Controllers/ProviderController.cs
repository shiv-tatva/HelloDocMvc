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

namespace HelloDocMVC.Controllers
{
    [CustomAuthorize("Provider")]
    public class ProviderController : Controller
    {
        private IProviderDash _IProviderDash;
        private IAdminDash _IAdminDash;

        public ProviderController (IProviderDash IProviderDash,IAdminDash IAdminDash)
        {
            _IProviderDash = IProviderDash;
            _IAdminDash = IAdminDash;
        }


        public IActionResult Provider()
        {
            var sessionName = HttpContext.Session.GetString("UserSessionName");
            TempData["headerUserName"] = sessionName;
            ViewBag.Admin = 5;

            var roleMain = HttpContext.Session.GetInt32("roleId");
            List<string> roleMenu = _IAdminDash.GetListOfRoleMenu((int)roleMain);
            ViewBag.Menu = roleMenu;
            return View();
        }



        //*************************************************Dashboard****************************************************************
        public IActionResult LoadPartialDashboard()
        {
            int[] status = { 1 };
            adminDashData adminDashObj = new adminDashData();
            var sessionName = HttpContext.Session.GetString("UserSession");
            adminDashObj._countMain = _IAdminDash.countService(sessionName);

            return PartialView("Provider/_ProviderDashboard", adminDashObj);
        }

        public IActionResult newTabTwo(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            adminDashObj._RegionTable = _IAdminDash.RegionTable();

            return PartialView("Provider/_ProviderNewTab", adminDashObj);
        }

        public IActionResult pendingTabTwo(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            adminDashObj._RegionTable = _IAdminDash.RegionTable();

            return PartialView("Provider/_ProviderTabPending", adminDashObj);
        }

        public IActionResult activeTabTwo(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            adminDashObj._RegionTable = _IAdminDash.RegionTable();


            return PartialView("Provider/_ProviderTabActive", adminDashObj);
        }

        public IActionResult concludeTabTwo(int[] arr)
        {
            int typeId = 0;
            int regionId = 0;
            adminDashData adminDashObj = new adminDashData();
            adminDashObj.data = _IAdminDash.adminData(arr, typeId, regionId);
            adminDashObj._RegionTable = _IAdminDash.RegionTable();


            return PartialView("Provider/_ProviderTabConclude", adminDashObj);
        }


        //*****************************************************My Profile******************************************************************************

        public IActionResult myProfile(int statusId, int flag)
        {
            var sessionEmail = HttpContext.Session.GetString("UserSession");
            adminDashData data = new adminDashData();
            data._providerEdit = _IAdminDash.adminEditPhysicianProfile(0, sessionEmail, flag , statusId);
            data._RegionTable = _IAdminDash.RegionTable();
            data._phyRegionTable = _IAdminDash.PhyRegionTable(data._providerEdit.PhyID);
            data._role = _IAdminDash.physicainRole();
            return PartialView("Provider/_ProviderMyProfile", data);
        }


        //*****************************************************Scheduling******************************************************************************

        //public IActionResult GetScheduling()
        //{
        //    SchedulingCm schedulingCm = new SchedulingCm()
        //    {
        //        regions = _IAdminDash.RegionTable(),
        //    };
        //    return PartialView("Scheduling/_Provider_Scheduling", schedulingCm);
        //}

    }
}
