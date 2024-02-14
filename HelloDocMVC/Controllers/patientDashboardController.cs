using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.AspNetCore.Http;
using static DAL_Data_Access_Layer_.CustomeModel.PatientDashboard;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelloDocMVC.Controllers
{
    public class patientDashboardController : Controller
    {
        private readonly IPatientDash _patientDashInfo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;


        public patientDashboardController(IPatientDash patientDashInfo,IWebHostEnvironment webHostEnvironment,ApplicationDbContext db)
        {
            _patientDashInfo = patientDashInfo;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult patientDashboard(string emaill)
        {

            if(HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("LoginPage","Login");
            }

            //IEnumerable<Request> list = _patientDashInfo.patientDashInfo(obj);

            // var list = _patientDashInfo.patientDashInfo();
            //var viewmodel = new PatidashboardInfo { patientDashboardItem = list };

            var result = from r1 in _db.Requests
                         join r2 in _db.Requestwisefiles on r1.Requestid equals r2.Requestid
                         group r1 by r2.Requestid into g
                         select new 
                         {
                             CreatedDate = g.Min(r => r.Createddate),
                             RequestTypeId = g.Min(r => r.Requesttypeid),
                             RequestId = g.Key,
                             FileCount = g.Count()
                         };            //var result = from r1 in dbContext.Requests
            //             join r2 in dbContext.RequestWiseFiles on r1.RequestId equals r2.RequestId
            //             group r2 by r1.RequestId into g
            //             select new
            //             {
            //                 CreatedDate = g.Min(r => r.CreatedDate),
            //                 RequestTypeId = g.Min(r => r.RequestTypeId),
            //                 RequestId = g.Key,
            //                 FileCount = g.Count()
            //             };            //var result=from t1 in _db.Requests join t2 in _db.Requestwisefiles on t1.Requestid equals t2.Requestid where t1.Email 

            //List<PatientDashboard> viewmodel = _patientDashInfo.patientDashInfo();

            ViewBag.Admin = 2; 
            return View(result);
        }
        

        
    }
}