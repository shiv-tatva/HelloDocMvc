using Assignment.Models;
using BusinessLayer.Interface;
using DataLayer.CustomeModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISchoolManagement schoolManagement;

        public HomeController(ILogger<HomeController> logger, ISchoolManagement _schoolManagement)
        {
            _logger = logger;
            schoolManagement = _schoolManagement;
        }



        /// <summary>
        /// This Page Is The landing page of the project
        /// </summary>
        /// <returns>returns main dashboard of project</returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var getDashboard = schoolManagement.GetDashboard();
                return View(getDashboard);
            }
            catch
            {
                return NotFound();
            }
            
        }


        /// <summary>
        /// This get method returns a modal,which is adding new students
        /// </summary>
        /// <returns>modal for add student</returns>
        [HttpGet]
        public IActionResult AddStudentModal()
        {
            try
            {
                return PartialView("_AddStudentModal");
            }
            catch
            {
                return NotFound();
            }           
        }



        /// <summary>
        /// This is the post method of Adding Student Modal
        /// </summary>
        /// <param name="schoolManagementCM"></param>
        /// <param name="exampleRadios"></param>
        /// <returns>Return Dashboard With Added Student</returns>
        [HttpPost]
        public IActionResult AddStudentModal(SchoolManagementCM schoolManagementCM,List<int> exampleRadios)
        {
            try
            {
                schoolManagement.AddStudent(schoolManagementCM, exampleRadios);                
                return RedirectToAction("Index");                
            }
            catch
            {
                return NotFound();
            }            
        }



        /// <summary>
        /// This Action is Gives a modal,with the help of we can edit student details
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>return edit modal</returns>
        [HttpGet]
        public IActionResult EditStudentDetails(int studentId)
        {
            try
            {
                var studentDetails = schoolManagement.GetStudentDetails(studentId);
                return PartialView("_EditStudentDetails", studentDetails);
            }
            catch
            {
                return NotFound();
            }
        }



        /// <summary>
        /// this is the post method of edit student details,which is helping us to edit student details
        /// </summary>
        /// <param name="schoolManagementCM"></param>
        /// <param name="exampleRadios"></param>
        /// <returns>return dashboard with edited details of student</returns>
        [HttpPost]
        public IActionResult EditStudentDetails(SchoolManagementCM schoolManagementCM, List<int> exampleRadios)
        {
            try
            {
                schoolManagement.EditStudentDetails(schoolManagementCM, exampleRadios);                
                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }           
        }



        /// <summary>
        /// this action gives us delete modal,which is take confirmation from users
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>return delete confirmation modal</returns>
        [HttpGet]
        public IActionResult DeletePopup(int studentId)
        {
            try
            {
                var deletePopup = schoolManagement.DeletePopup(studentId);
                return PartialView("_DeleteConfirmation", deletePopup);
            }
            catch
            {
                return NotFound();
            }


        }
        


        /// <summary>
        /// this is the post method of delete button,which is helping us to delete students information
        /// </summary>
        /// <param name="addStudent"></param>
        /// <returns>delete student information</returns>
        [HttpPost]
        public IActionResult DeletePopup(AddStudent addStudent)
        {
            try
            {
                schoolManagement.DeleteConfirm(addStudent);
                return Ok();
            }
            catch
            {
                return NotFound();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}