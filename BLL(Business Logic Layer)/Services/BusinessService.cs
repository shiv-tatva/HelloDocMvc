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

namespace BLL_Business_Logic_Layer_.Services
{
    public class BusinessService : IBusiness
    {
        private readonly ApplicationDbContext _context;

        public BusinessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SendRegistrationEmail(string toEmail, string registrationLink)
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
                Subject = "Update Password for Account ",
                IsBodyHtml = true,
                Body = $"Click the following link to complete your registration: <a href='{registrationLink}'>{registrationLink}</a>"
            };



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
                _user.State = obj.state;
                _user.Zipcode = obj.zipcode;
                _user.Strmonth = obj.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));
                _user.Createddate = DateTime.Now;

                _context.Users.Add(_user);
                _context.SaveChanges();
            }

            

            var user = _context.Users.FirstOrDefault(x => x.Email == obj.email);

            _request.Requesttypeid = 4;
            _request.Userid = _user.Userid;
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

            var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);

            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = obj.firstname;
            _requestclient.Lastname = obj.lastname;
            _requestclient.Email = obj.email;
            _requestclient.Phonenumber = obj.phone;
            _requestclient.Notes = obj.symptoms;
            _requestclient.Street = obj.street;
            _requestclient.City = obj.city;
            _requestclient.State = obj.state;
            _requestclient.Zipcode = obj.zipcode;
            _requestclient.Strmonth = obj.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(obj.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(obj.dateofbirth.Substring(0, 4));

            if (userexist == null)
                {
                    string emailConfirmationToken = Guid.NewGuid().ToString();

                    string registrationLink = "http://localhost:5145/Home/CreateAccount";

                    //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                    try
                    {
                        SendRegistrationEmail(obj.email, registrationLink);
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
