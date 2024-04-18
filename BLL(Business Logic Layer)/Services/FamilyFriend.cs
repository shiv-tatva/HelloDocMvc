using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using HelloDocMVC.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BLL_Business_Logic_Layer_.Services
{
    public class FamilyFriend : IFamilyFriend
    {
        private readonly ApplicationDbContext _context;

        public FamilyFriend  (ApplicationDbContext context)
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



        public void FamilyFriendInfo(FamilyFriendData data)
        {
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();
            User _user = new User();
            Aspnetuser _aspnetuser = new Aspnetuser();
            Aspnetuserrole _role = new Aspnetuserrole();

            var user = _context.Aspnetusers.FirstOrDefault(x => x.Email == data.email);

            if (user == null)
            {
                _aspnetuser.Username = data.firstname + "_" + data.lastname;
                _aspnetuser.Email = data.email;
                _aspnetuser.Phonenumber = data.phone;
                _aspnetuser.Createddate = DateTime.Now;

                _context.Aspnetusers.Add(_aspnetuser);
                _context.SaveChanges();

                _user.Aspnetuserid = _aspnetuser.Id;
                _user.Createdby = _aspnetuser.Id;
                _user.Firstname = data.firstname;
                _user.Lastname = data.lastname;
                _user.Email = data.email;
                _user.Mobile = data.phone;
                _user.Street = data.street;
                _user.City = data.city;
                _user.State = _context.Regions.Where(r => r.Regionid == data.regionId).Select(r => r.Name).First();
                _user.Zipcode = data.zipcode;
                _user.Strmonth = data.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                _user.Createddate = DateTime.Now;
                _user.Regionid = data.regionId;

                _context.Users.Add(_user);
                _context.SaveChanges();

                _role.Userid = _aspnetuser.Id;
                _role.Roleid = 2;

                _context.Aspnetuserroles.Add(_role);
                _context.SaveChanges();
            }

            _request.Requesttypeid = 2;
            if (user == null)
            {
                _request.Userid = _user.Userid;
            }
            else
            {
                var a = _context.Users.Where(r => r.Email == data.email).Select(r => r.Userid).First();
                _request.Userid = a;
            }

            _request.Firstname = data.ff_firstname;
            _request.Lastname = data.ff_lastname;
            _request.Phonenumber = data.ff_phone;
            _request.Email = data.ff_email;
            _request.Relationname = data.ff_relation;
            _request.Confirmationnumber = data.firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
            _request.Createddate = DateTime.Now;
            _request.Status = 1;
           
            _context.Requests.Add(_request);
            _context.SaveChanges();

            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = data.firstname;
            _requestclient.Lastname = data.lastname;
            _requestclient.Phonenumber = data.phone;
            _requestclient.Email = data.email;
            _requestclient.Notes = data.symptoms;
            _requestclient.Street = data.street;
            _requestclient.City = data.city;
            _requestclient.State = _context.Regions.Where(r => r.Regionid == data.regionId).Select(r => r.Name).First();
            _requestclient.Zipcode = data.zipcode;
            _requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
            _requestclient.Regionid = data.regionId;

            if (user == null)
                {
                    string emailConfirmationToken = Guid.NewGuid().ToString();

                string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _aspnetuser.Id;

                //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                try
                    {
                        SendRegistrationEmail(data.email, registrationLink, _request.Email);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                              
            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

        }
    }
}
