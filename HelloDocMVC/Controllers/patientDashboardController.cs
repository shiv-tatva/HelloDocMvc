﻿using HelloDocMVC.Models;
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
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;
using BLL_Business_Logic_Layer_.Services;
using NuGet.Protocol;


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
                PatientDashboard patientDashboard = new PatientDashboard();
                string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
                patientDashboard.data = _patientDashInfo.patientDashInfo(emailpatient);

                ViewBag.Admin = 2;
                return View(patientDashboard);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }

        }



        //public IActionResult logoutSession()
        //{

        //    HttpContext.Session.Clear();
        //    return RedirectToAction("LoginPage", "Login");

        //}

       


      

        public IActionResult DownloadFile(string data)
        {
            
            if (HttpContext.Session.GetString("UserSession").ToString() != null)
            {
                string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data);
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathname);//filepath will relative as per the filename
                string fileName = data;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, data);
            }
            else 
            {
                return RedirectToAction("LoginPage", "Login");
            }

        }


        public IActionResult profileMain()
        {

            if (HttpContext.Session.GetString("UserSession").ToString() != null)
            {
                PatientDashboardData patientDashboard = new PatientDashboardData();
                string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
                PatientDashboardData data = _patientDashInfo.UserProfile(emailpatient);

                ViewBag.Admin = 2;
                return View(data);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }

        }

        [HttpPost]
        public IActionResult profileMain(PatientDashboardData p)
        {

           var userEmail = HttpContext.Session.GetString("UserSession").ToString();
            var request = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (request != null)
            {
                request.Email = userEmail;
                request.Street = p.street;
                request.City = p.city;
                request.State = p.state;
                request.Zipcode = p.zipcode;
                request.Firstname = p.fname;
                request.Lastname = p.lname;
                request.Mobile = p.phone_no;

            }

            _db.SaveChanges();
            ViewBag.Admin = 2;

            PatientDashboardData viewUpdated_data = new PatientDashboardData();

            User updateduserdata=_db.Users.FirstOrDefault(x => x.Email == userEmail);

            if (updateduserdata != null)
            {

                viewUpdated_data.fname = updateduserdata.Firstname;

                viewUpdated_data.lname = updateduserdata.Lastname;
                viewUpdated_data.phone_no = updateduserdata.Mobile;
                viewUpdated_data.street = updateduserdata.Street;
                viewUpdated_data.state = updateduserdata.State;
                viewUpdated_data.city = updateduserdata.City;
                viewUpdated_data.zipcode = updateduserdata.Zipcode;
                viewUpdated_data.email = updateduserdata.Email;
            }
            return View(viewUpdated_data);

        }

       
        public IActionResult dashboardMeView()
        {
            if (HttpContext.Session.GetString("UserSession").ToString() != null)
            {

                var userEmail = HttpContext.Session.GetString("UserSession").ToString();
                Custom data = _patientDashInfo.userMeDetail(userEmail);

                ViewBag.Admin = 2;
                return View(data);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }
        
        
       


        [HttpPost]
        public IActionResult dashboardMeView(Custom obj)
        {
            _patientDashInfo.userDetail(obj);
            ViewBag.Admin = 2;
            return RedirectToAction("patientDashboard");
        }
        
        
        public IActionResult dashboardSomeOneView()
        {
            if (HttpContext.Session.GetString("UserSession").ToString() != null)
            {

                var userEmail = HttpContext.Session.GetString("UserSession").ToString();
                FamilyFriendData data = _patientDashInfo.userSomeDetail(userEmail);

                ViewBag.Admin = 2;
                return View(data);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }


        [HttpPost]
        public IActionResult dashboardSomeOneView(FamilyFriendData obj)
        {
            _patientDashInfo.userSomeOneDetail(obj);
            ViewBag.Admin = 2;
            return RedirectToAction("patientDashboard");
        }

        public IActionResult viewDetail(int param)
        {

            if (HttpContext.Session.GetString("UserSession").ToString() != null)
            {

                PatientDashboard patientDashboard = new PatientDashboard();

                string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
                patientDashboard.data = _patientDashInfo.patientDashInfoTwo(emailpatient, param);
                ViewData["id"] = param;


                ViewBag.Admin = 2;
                return View(patientDashboard);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }

        }

        [HttpPost]
        public IActionResult viewDetail(PatientDashboard obj)
        {
            _patientDashInfo.viewDocumentUpload(obj);
            ViewBag.Admin = 2;
            var param = obj.data[0].reqid;
            return RedirectToAction("viewDetail", new {param = param});
        }
        public IActionResult logoutSession()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage", "Login");
        }



    }
}