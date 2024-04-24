using BLL_Business_Logic_Layer_.Interface;
using BusinessLogic.Interfaces;
using DAL_Data_Access_Layer_.CustomeModel;
using HelloDocMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HelloDocMVC.Controllers
{

    //[Route("api/[controller]")]
    //[ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginService _login;
        private IConfiguration _config;
        private IJwtService _jwtService;

        //ApplicationDbContext db = new ApplicationDbContext();

        public LoginController(ILoginService _login, IConfiguration config, IJwtService _jwtService)
        {
            this._login = _login;
            this._jwtService = _jwtService;
            _config = config;
        }


        public IActionResult LoginPage()
        {
            ViewBag.Admin = 1;
            return View();
        }


        private string GenerateToken(string Email, string PasswordHash)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginPage(Users obj)
        {

            var data = _login.login(obj);

            IActionResult response = Unauthorized();


            //TempData["email"] = obj.Email;
            var sessionUser = obj.Email;
            var userName = obj.Email.Substring(0, obj.Email.IndexOf('@'));

            ViewBag.Admin = 1;

            if (data.passwordcheck == "true")
            {
                HttpContext.Session.SetString("UserSession", sessionUser);
                HttpContext.Session.SetString("UserSessionName", userName);
                HttpContext.Session.SetInt32("AspNetUserID", (int)data.Id);
                HttpContext.Session.SetInt32("roleId", data.roleId);


                var jwtToken = _jwtService.GetJwtToken(data);
                Response.Cookies.Append("jwt", jwtToken);

                TempData["name"] = data.Username;



                if (data.RoleMain == "User")
                {
                    TempData["success"] = "Login Successfully!";
                    return RedirectToAction("patientDashboard", "patientDashboard");
                }
                else if (data.RoleMain == "Admin")
                {
                    TempData["success"] = "Login Successfully!";
                    return RedirectToAction("adminDashboard", "adminDashboard");
                }
                else if (data.RoleMain == "Provider")
                {
                    TempData["success"] = "Login Successfully!";
                    return RedirectToAction("Provider", "Provider");
                }
                else
                {
                    return View();
                }

            }
            else
            {
                if (data.emailcheck == "emailFalse" && data.passwordcheck == "passwordFalse")
                {
                    TempData["email"] = "Email is incorrect";
                    TempData["password"] = "Password is incorrect";
                }
                if (data.emailcheck == "emailFalse")
                {
                    TempData["email"] = "Email is incorrect";
                }
                else if (data.passwordcheck == "passwordFalse")
                {
                    TempData["password"] = "Password is incorrect";
                }

                TempData["error"] = "Incorrect Details!";
                return View();
            }


            ////return View();

            //return response;

        }
        public IActionResult ForgotPage()
        {
            ViewBag.Admin = 1;
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ForgotPasswordPage(Users obj)
        {
            var user = _login.forgotPassword(obj);
            return Json(new { isSend = user.flagId });
        }
    }
}
