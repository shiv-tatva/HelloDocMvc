using HelloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using BLL_Business_Logic_Layer_.Interface;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.CustomeModel;

namespace HelloDocMVC.Controllers
{
    public class patientDashboardController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public IActionResult patientDashboard()
        {

            var data = _db.Requests.ToList();

            ViewBag.Admin = 2; 
            return View(data);
        }
    }
}