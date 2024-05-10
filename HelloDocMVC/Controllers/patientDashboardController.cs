using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using HalloDoc.mvc.Auth;
using HelloDocMVC.CustomeModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloDocMVC.Controllers
{
    [CustomAuthorize("User")]
    public class patientDashboardController : Controller
    {
        private readonly IPatientDash _patientDashInfo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        private IAdminDash _IAdminDash;


        public patientDashboardController(IPatientDash patientDashInfo, IWebHostEnvironment webHostEnvironment, ApplicationDbContext db, IAdminDash iAdminDash)
        {
            _patientDashInfo = patientDashInfo;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _IAdminDash = iAdminDash;
        }



        /// <summary>
        /// this action is for patientDashboard
        /// </summary>
        /// <returns></returns>
        public IActionResult patientDashboard()
        {
            try
            {
                ViewBag.Mysession = HttpContext.Session.GetString("UserSession").ToString();
                PatientDashboard patientDashboard = new PatientDashboard();
                string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
                patientDashboard.data = _patientDashInfo.patientDashInfo(emailpatient);
                patientDashboard.adminData = _patientDashInfo.adminDataMain();
                ViewBag.Admin = 2;
                return View(patientDashboard);
            }
            catch
            {
                return NotFound();
            }

        }



        /// <summary>
        /// this action is for DownloadFile
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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




        /// <summary>
        /// this action is for profile
        /// </summary>
        /// <returns></returns>
        public IActionResult profileMain()
        {
            try
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
            catch
            {
                return NotFound();
            }

        }



        /// <summary>
        /// this action is for profile
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult profileMain(PatientDashboardData p)
        {
            try
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
                    request.Strmonth = p.fulldateofbirth.Substring(5, 2);
                    request.Intdate = Convert.ToInt16(p.fulldateofbirth.Substring(8, 2));
                    request.Intyear = Convert.ToInt16(p.fulldateofbirth.Substring(0, 4));
                }

                _db.SaveChanges();

                return RedirectToAction("profileMain");
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for dashboard view(ME)
        /// </summary>
        /// <returns></returns>
        public IActionResult dashboardMeView()
        {
            try
            {
                if (HttpContext.Session.GetString("UserSession").ToString() != null)
                {

                    var userEmail = HttpContext.Session.GetString("UserSession").ToString();
                    Custom data = _patientDashInfo.userMeDetail(userEmail);
                    data._RegionTable = _IAdminDash.RegionTable();

                    ViewBag.Admin = 2;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("LoginPage", "Login");
                }
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for dashboard me view
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult dashboardMeView(Custom obj)
        {
            try
            {
                _patientDashInfo.userDetail(obj);
                ViewBag.Admin = 2;
                return RedirectToAction("patientDashboard");
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for dashboard someone view
        /// </summary>
        /// <returns></returns>
        public IActionResult dashboardSomeOneView()
        {
            try
            {
                if (HttpContext.Session.GetString("UserSession").ToString() != null)
                {

                    var userEmail = HttpContext.Session.GetString("UserSession").ToString();
                    FamilyFriendData data = new FamilyFriendData();
                    data._RegionTable = _IAdminDash.RegionTable();

                    ViewBag.Admin = 2;
                    return View(data);
                }
                else
                {
                    return RedirectToAction("LoginPage", "Login");
                }
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for dashboardSomeOneView
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult dashboardSomeOneView(FamilyFriendData obj)
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserSession").ToString();
                _patientDashInfo.userSomeOneDetail(obj, userEmail);
                ViewBag.Admin = 2;
                return RedirectToAction("patientDashboard");
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for SubmitElseInfo
        /// </summary>
        /// <returns></returns>
        public IActionResult SubmitElseInfo()
        {
            return View();
        }




        /// <summary>
        /// this action is for viewDetail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IActionResult viewDetail(int param)
        {
            try
            {
                if (HttpContext.Session.GetString("UserSession").ToString() != null)
                {

                    PatientDashboard patientDashboard = new PatientDashboard();

                    string emailpatient = HttpContext.Session.GetString("UserSession").ToString();
                    string emailpatientName = HttpContext.Session.GetString("UserSessionName").ToString();
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
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for viewDetail
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult viewDetail(PatientDashboard obj)
        {
            try
            {
                _patientDashInfo.viewDocumentUpload(obj);
                ViewBag.Admin = 2;
                var param = obj.data[0].reqid;
                return RedirectToAction("viewDetail", new { param = param });
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for pendingReviewAgreement
        /// </summary>
        /// <param name="reqId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult pendingReviewAgreement(int reqId)
        {
            try
            {
                ViewBag.Admin = 1;
                PatientDashboard patientDashboardMain = new PatientDashboard();
                patientDashboardMain._reviewAgreement = _patientDashInfo.reviewAgree(reqId);
                return View(patientDashboardMain);
            }
            catch
            {
                return NotFound();
            }

        }




        /// <summary>
        /// this action is for pendingReviewAgreement
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult pendingReviewAgreement(PatientDashboard obj)
        {
            try
            {
                if (obj._reviewAgreement.flag == 1)
                {
                    if (_patientDashInfo.checkstatus(obj._reviewAgreement.reqid) == false)
                    {
                        _patientDashInfo.reviewAgree(obj);
                        TempData["success"] = "Status changed Successfully!";
                    }
                    else
                    {
                        TempData["error"] = "Status Already changed!";
                    }
                }
                else
                {
                    if (_patientDashInfo.checkstatus(obj._reviewAgreement.reqid) == false)
                    {
                        _patientDashInfo.agreeMain(obj._reviewAgreement.reqid);
                        TempData["success"] = "Status changed Successfully!";
                    }
                    else
                    {
                        TempData["error"] = "Status Already changed!";
                    }
                }
                return RedirectToAction("pendingReviewAgreement", new { reqId = obj._reviewAgreement.reqid });
            }
            catch
            {
                return NotFound();
            }

        }
    }
}