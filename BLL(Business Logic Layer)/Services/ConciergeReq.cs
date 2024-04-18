using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Collections;

namespace BLL_Business_Logic_Layer_.Services
{
    public class ConciergeReq : IConcierge
    {
        private readonly ApplicationDbContext _db;

        public ConciergeReq(ApplicationDbContext context)
        {
            _db = context;
        }


        public void SendRegistrationEmail(string toEmail, string registrationLink,string ReqEmail)
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
                Subject = "Create Account ",
                IsBodyHtml = true,
                Body = $"Click the following link to complete your registration: <a href='{registrationLink}'>{registrationLink}</a>"
            };


            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + toEmail + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = toEmail,
                Roleid = 2,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = ReqEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (_db.Requests.Any(r => r.Email == ReqEmail))
            {
                emailLog.Requestid = _db.Requests.Where(r => r.Email == ReqEmail).Select(r => r.Requestid).First();
            }


            _db.Emaillogs.Add(emailLog);
            _db.SaveChanges();

            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }

        public void ConciergeDetail(ConciergeCustom obj)
        {
            Concierge _concierge = new Concierge();
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();
            User _user = new User();
            Aspnetuser _aspnetuser = new Aspnetuser();
            Aspnetuserrole _role = new Aspnetuserrole();

            var user = _db.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

            if (user == null)
            {
                _aspnetuser.Username = obj.firstname + "_" + obj.lastname;
                _aspnetuser.Email = obj.email;
                _aspnetuser.Phonenumber = obj.phone;
                _aspnetuser.Createddate = DateTime.Now;

                _db.Aspnetusers.Add(_aspnetuser);
                _db.SaveChanges();

                _user.Aspnetuserid = _aspnetuser.Id;
                _user.Createdby = _aspnetuser.Id;
                _user.Firstname = obj.firstname;
                _user.Lastname = obj.lastname;
                _user.Email = obj.email;
                _user.Mobile = obj.phone;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Createddate = DateTime.Now;
                _user.Street = obj.concierge_street;
                _user.City = obj.concierge_city;
                _user.State = _db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
                _user.Zipcode = obj.concierge_zipcode;
                _user.Regionid = obj.regionId;

                _db.Users.Add(_user);
                _db.SaveChanges();

                _role.Userid = _aspnetuser.Id;
                _role.Roleid = 2;

                _db.Aspnetuserroles.Add(_role);
                _db.SaveChanges();
            }

            _concierge.Conciergename = obj.concierge_firstname;
            _concierge.Street = obj.concierge_street;
            _concierge.City = obj.concierge_city;
            _concierge.State = _db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
            _concierge.Zipcode = obj.concierge_zipcode;
            _concierge.Createddate = DateTime.Now;
            _db.Concierges.Add( _concierge );
            _db.SaveChanges();

            var userMain = _db.Users.FirstOrDefault(x => x.Email == obj.email);

            if (user == null)
            {
                _request.Userid = _user.Userid;
            }
            else
            {
                var a = _db.Users.Where(r => r.Email == obj.email).Select(r => r.Userid).First();
                _request.Userid = a;
            }

            _request.Requesttypeid = 3;
            _request.Firstname = obj.concierge_firstname;
            _request.Lastname = obj.concierge_lastname;
            _request.Phonenumber = obj.concierge_phone;
            _request.Email = obj.concierge_email;
            _request.Confirmationnumber = obj.concierge_firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
            _request.Status = 1;
            _request.Createddate = DateTime.Now;

            _db.Requests.Add( _request );
            _db.SaveChanges();


            
             _requestclient.Requestid = _request.Requestid;
             _requestclient.Firstname = obj.firstname;
             _requestclient.Lastname = obj.lastname;
             _requestclient.Phonenumber = obj.phone;
             _requestclient.Email = obj.email;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear =  Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.Regionid = obj.regionId;
            _requestclient.City = obj.concierge_city;
            _requestclient.Street = obj.concierge_street;
            _requestclient.Zipcode = obj.concierge_zipcode;
            _requestclient.Notes = obj.symptoms;
            _requestclient.State = _db.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();


            if (user == null)
                {
                    string emailConfirmationToken = Guid.NewGuid().ToString();

                string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _aspnetuser.Id;

                //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                try
                    {
                        SendRegistrationEmail(obj.email, registrationLink, obj.concierge_email);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            _db.Requestclients.Add(_requestclient);
            _db.SaveChanges();
        }
    }
}
