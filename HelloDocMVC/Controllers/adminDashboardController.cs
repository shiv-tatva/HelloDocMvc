using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.AspNetCore.Mvc;

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
    }
}
