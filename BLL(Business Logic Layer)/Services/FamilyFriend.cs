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

namespace BLL_Business_Logic_Layer_.Services
{
    public class FamilyFriend : IFamilyFriend
    {
        private readonly ApplicationDbContext _context;

        public FamilyFriend  (ApplicationDbContext context)
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



        public void FamilyFriendInfo(FamilyFriendData data)
        {
            Request _request = new Request();
            Requestclient _requestclient = new Requestclient();
            User _user = new User();
            Aspnetuser _aspnetuser = new Aspnetuser();

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
                _user.State = data.state;
                _user.Zipcode = data.zipcode;
                _user.Strmonth = data.dateofbirth.Substring(5, 2);
                _user.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
                _user.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                _user.Createddate = DateTime.Now;

                _context.Users.Add(_user);
                _context.SaveChanges();
            }

            _request.Requesttypeid = 2;
            _request.Userid = _user.Userid;
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

            var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == data.email);


            _requestclient.Requestid = _request.Requestid;
            _requestclient.Firstname = data.firstname;
            _requestclient.Lastname = data.lastname;
            _requestclient.Phonenumber = data.phone;
            _requestclient.Email = data.email;
            _requestclient.Notes = data.symptoms;
            _requestclient.Street = data.street;
            _requestclient.City = data.city;
            _requestclient.State = data.state;
            _requestclient.Zipcode = data.zipcode;
            _requestclient.Strmonth = data.dateofbirth.Substring(5, 2);
            _requestclient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
            _requestclient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));

            if (userexist == null)
                {
                    string emailConfirmationToken = Guid.NewGuid().ToString();

                    string registrationLink = "http://localhost:5145/Home/CreateAccount";

                    //string registrationLink = $"/Home/CreateAccount?token={emailConfirmationToken}";

                    try
                    {
                        SendRegistrationEmail(data.email, registrationLink);
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
