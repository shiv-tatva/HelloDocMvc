using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;

namespace BLL_Business_Logic_Layer_.Services
{
    public class BusinessService : IBusiness
    {
        private readonly ApplicationDbContext _context;

        public BusinessService(ApplicationDbContext context)
        {
            _context = context;
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

            if (_context.Requests.Any(r => r.Email == ReqEmail))
            {
                emailLog.Requestid = _context.Requests.Where(r => r.Email == ReqEmail).Select(r => r.Requestid).First();
            }


            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();


            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }
        public void businessInfo(BusinessCustome obj)
        {
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();  
            Business _business = new Business();
            Requestbusiness _requestbusiness = new Requestbusiness();
            User _user = new User();
            Aspnetuser _aspnetuser = new Aspnetuser();
            Aspnetuserrole _role = new Aspnetuserrole();

            var userFatch = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

            if (userFatch == null)
            {
                _aspnetuser.Username = obj.firstname + "_" + obj.lastname;
                _aspnetuser.Email = obj.email;
                _aspnetuser.Phonenumber = obj.phone;
                _aspnetuser.Createddate = DateTime.Now;

                _context.Aspnetusers.Add(_aspnetuser);
                _context.SaveChanges();

                _user.Aspnetuserid = _aspnetuser.Id;
                _user.Createdby = _aspnetuser.Id;
                _user.Firstname = obj.firstname;
                _user.Lastname = obj.lastname;
                _user.Email = obj.email;
                _user.Mobile = obj.phone;
                _user.Street = obj.street;
                _user.City = obj.city;
                _user.State = _context.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
                _user.Zipcode = obj.zipcode;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Regionid = obj.regionId;
                _user.Createddate = DateTime.Now;

                _context.Users.Add(_user);
                _context.SaveChanges();

                _role.Userid = _aspnetuser.Id;
                _role.Roleid = 2;

                _context.Aspnetuserroles.Add(_role);
                _context.SaveChanges();
            }

            

            var user = _context.Users.FirstOrDefault(x => x.Email == obj.email);

            _request.Requesttypeid = 4;
            if (userFatch == null)
            {
                _request.Userid = _user.Userid;
            }
            else
            {
                var a = _context.Users.Where(r => r.Email == obj.email).Select(r => r.Userid).First();
                _request.Userid = a;
            }
            _request.Firstname = obj.business_firstname;
            _request.Lastname = obj.business_lastname;
            _request.Phonenumber = obj.business_phone;
            _request.Email = obj.business_email;
            _request.Casenumber = obj.business_casenumber;
            _request.Status = 1;
            _request.Confirmationnumber = obj.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
            _request.Createddate = DateTime.Now;

            _context.Requests.Add(_request);
            _context.SaveChanges();


            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = obj.firstname;
            _requestclient.Lastname = obj.lastname;
            _requestclient.Email = obj.email;
            _requestclient.Phonenumber = obj.phone;
            _requestclient.Notes = obj.symptoms;
            _requestclient.Street = obj.street;
            _requestclient.City = obj.city;
            _requestclient.State = _context.Regions.Where(r => r.Regionid == obj.regionId).Select(r => r.Name).First();
            _requestclient.Zipcode = obj.zipcode;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
            _requestclient.Regionid = obj.regionId;

            if (userFatch == null)
                {
                    string emailConfirmationToken = Guid.NewGuid().ToString();

                string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _aspnetuser.Id;

                //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                try
                    {
                        SendRegistrationEmail(obj.email, registrationLink, obj.business_email);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                
            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

            
            _business.Name = obj.business_property;
            _business.Createddate = DateTime.Now;

            _context.Businesses.Add(_business);
            _context.SaveChanges();

            _requestbusiness.Requestid = _request.Requestid;
            _requestbusiness.Businessid = _business.Businessid;

            _context.Requestbusinesses.Add(_requestbusiness);
            _context.SaveChanges();


        }
    }
}
