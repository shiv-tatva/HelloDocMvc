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


        public IActionResult patientDashboard()
        {

            if(HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("LoginPage","Login");
            }

          

            PatientDashboard patientDashboard = new PatientDashboard();
            string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
            patientDashboard.data = _patientDashInfo.patientDashInfo(emailpatient);
          
            ViewBag.Admin = 2; 
            return View(patientDashboard);
        }
        
        public IActionResult viewDetail()
        {
            PatientDashboard patientDashboard = new PatientDashboard();
            string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
            patientDashboard.data = _patientDashInfo.patientDashInfo(emailpatient);

            ViewBag.Admin = 2;
            return View(patientDashboard);

        }

        
    }
}