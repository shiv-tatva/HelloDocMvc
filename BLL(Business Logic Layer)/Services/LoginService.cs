using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataModels;
using DAL_Data_Access_Layer_.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Data_Access_Layer_.CustomeModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace BLL_Business_Logic_Layer_.Services
{ 
    public class LoginService : ILoginService
    {

        private readonly ApplicationDbContext db;

        public LoginService(ApplicationDbContext _db)
        {
               db = _db;
        }

        public Users login(Users obj)
        {
            Users users = new Users();

            var checkRoleId = db.Admins.FirstOrDefault(r => r.Email == obj.Email);
            var checkRoleIdTwo = db.Physicians.FirstOrDefault(r => r.Email == obj.Email);

            var roleIdMain = 0;

            if (checkRoleId != null)
            {
                roleIdMain = db.Admins.Where(r => r.Email == obj.Email).Select(x => (int)x.Roleid).First();
            }
            if (checkRoleIdTwo != null)
            {
                roleIdMain = db.Physicians.Where(r => r.Email == obj.Email).Select(x => (int)x.Roleid).First();
            }

            var check = db.Aspnetusers.Where(x => x.Email == obj.Email && x.Passwordhash == obj.Passwordhash).FirstOrDefault();
            if(check != null)
            {
                var data = db.Aspnetusers.Where(x => x.Email == obj.Email && x.Passwordhash == obj.Passwordhash).Select(r => new Users()
                {
                    Id = r.Id,
                    passwordcheck = "true",
                    Email = obj.Email,
                    Passwordhash = obj.Passwordhash,
                    RoleMain = db.Aspnetroles.Where(y => y.Id == db.Aspnetuserroles.Where(x => x.Userid == r.Id).Select(x => x.Roleid).First()).Select(y => y.Name).First(),
                    roleId = (int)roleIdMain,
                }).ToList().FirstOrDefault();
                return data;
            }
            else
            {
               

                 if(db.Aspnetusers.Where(r => r.Email == obj.Email).FirstOrDefault() == null)
                {
                    users.emailcheck = "emailFalse";
                }
                else
                {
                    users.passwordcheck = "passwordFalse";
                }
                return users;
            }

        }

        public Users forgotPassword(Users obj)
        {
            Users _user = new Users();

            var emailMain = db.Aspnetusers.Any(x => x.Email == obj.Email);

            if(emailMain == true)
            {
                var emailMainTwo = db.Aspnetusers.Where(x => x.Email == obj.Email).Select(r => r).First();

                string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + emailMainTwo.Id;

                try
                {
                    SendRegistrationEmailCreateRequest(emailMainTwo.Email, registrationLink);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                _user.flagId = 1;
                return _user;
            }

            _user.flagId = 2;
            return _user;
        }

        public void SendRegistrationEmailCreateRequest(string email, string registrationLink)
        {
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Create Account",
                IsBodyHtml = true,
                Body = $"Click the following link to Create Account: <a href='{registrationLink}'>{registrationLink}</a>"
            };



            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }
    }
}
