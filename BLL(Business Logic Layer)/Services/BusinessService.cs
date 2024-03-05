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

            _request.Requesttypeid = 4;

            var user = _context.Users.FirstOrDefault(x => x.Email == obj.email);

            if(user != null)
            {
                _request.Userid = user.Userid;
            }

            if (obj.business_firstname != null)
            {
                _request.Firstname = obj.business_firstname;
            }
            
            if (obj.business_lastname != null)
            {
                _request.Lastname = obj.business_lastname;
            }
            
            if (obj.business_phone != null)
            {
                _request.Phonenumber = obj.business_phone;
            }
            
            if (obj.business_email != null)
            {
                _request.Email = obj.business_email;
            }
             
            if (obj.business_casenumber != null)
            {
                _request.Casenumber = obj.business_casenumber;
            }

           
            _context.Requests.Add(_request);
            _context.SaveChanges();

            var userexist = _context.Aspnetusers.FirstOrDefault(x => x.Email == obj.email);


            if (_request.Requestid != null)
            {
                _requestclient.Requestid = _request.Requestid;
            }
            
            if(obj.firstname != null)
            {
                _requestclient.Firstname = obj.firstname;
            }
            
            if(obj.lastname != null)
            {
                _requestclient.Lastname = obj.lastname;
            }
              
            if(obj.phone != null)
            {
                _requestclient.Phonenumber = obj.phone;
            }

            if (obj.email != null)
            {
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

                _requestclient.Email = obj.email;
            }


            _context.Requestclients.Add(_requestclient);
            _context.SaveChanges();

            if (obj.business_property != null)
            {
                _business.Name = obj.business_property;
            }
            _business.Createddate = DateTime.Now;
            _context.Businesses.Add(_business);
            _context.SaveChanges();


            if(_request.Requestid != null)
            {
                _requestbusiness.Requestid = _request.Requestid;
            }
            
            if(_business.Businessid != null)
            {
                _requestbusiness.Businessid = _business.Businessid;
            }

            _context.Requestbusinesses.Add(_requestbusiness);
            _context.SaveChanges();


        }
    }
}
